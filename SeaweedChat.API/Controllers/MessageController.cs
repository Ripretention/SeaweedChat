using SeaweedChat.Domain.Aggregates;
using Microsoft.AspNetCore.Mvc;
using SeaweedChat.API.Models;
namespace SeaweedChat.API.Controllers;

[Route("api/Chats/{ChatId:guid}/[controller]s")]
public class MessageController : ApiController
{
    private readonly IMessageRepository _msgRepository;
    private readonly IChatRepository _chatRepository;
    public MessageController(
        IChatRepository chatRepository,
        IMessageRepository msgRepository,
        IUserRepository usrRepository,
        ILogger<ChatController> logger
    ) : base(logger, usrRepository)
    {
        _chatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
        _msgRepository = msgRepository ?? throw new ArgumentNullException(nameof(msgRepository));
    }

    public Guid CurrentChatId
    {
        get 
        {
            Guid.TryParse((string?)RouteData.Values.FirstOrDefault(v => v.Key == "ChatId").Value ?? "", out Guid chatId);
            return chatId;
        }
    }

    [HttpPost("{msgId:guid}")]
    public async Task<ActionResult<EditMessageResponse>> EditMessage([FromRoute] Guid msgId, [FromBody] EditMessageRequest request)
    {
        var chat = await _chatRepository.Get(CurrentChatId);
        if (chat?.Members?.All(m => m.Id != CurrentUserId) ?? true)
            return BadRequest(new EditMessageResponse
            {
                Message = "Unknown chat"
            });
        var msg = await _msgRepository.Get(msgId);
        if (msg == null || msg.Chat.Id != CurrentChatId)
            return BadRequest(new EditMessageResponse
            {
                Message = "Unknown message"
            });

        if (request.Text != null)
        {
            msg.Text = request.Text;
            msg.EditAt = DateTime.Now;
        }

        await _chatRepository.Update();
        return Ok(new EditMessageResponse
        {
            Result = true,
            Message = "Success"
        });
    }

    [HttpGet]
    public async Task<ActionResult<GetAllMessageResponse>> GetMessages([FromQuery] int offset = 0, [FromQuery] int limit = 200)
    {
        var chat = await _chatRepository.Get(CurrentChatId);
        if (chat?.Members?.All(m => m.Id != CurrentUserId) ?? true)
            return BadRequest(new GetChatResponse
            {
                Message = "Unknown chat"
            });

        var messages = await _msgRepository.GetChatMessages(chat, offset, limit);
        return Ok(new GetAllMessageResponse
        {
            Result = true,
            Message = "Success",
            Messages = messages
        });
    }
    [HttpGet("{msgId:guid}")]
    public async Task<ActionResult<GetMessageResponse>> GetMessage(Guid msgId)
    {
        var chat = await _chatRepository.Get(CurrentChatId);
        if (chat?.Members?.All(m => m.Id != CurrentUserId) ?? true)
            return BadRequest(new GetChatResponse
            {
                Message = "Unknown chat"
            });

        var msg = await _msgRepository.Get(msgId);
        if (msg == null || msg.Chat != chat)
            return BadRequest(new GetMessageResponse
            {
                Message = "Unknown message"
            });

        return Ok(new GetMessageResponse
        {
            Result = true,
            Message = "Success",
            MessageBody = msg
        });
    }
    [HttpPut]
    public async Task<ActionResult<AddMessageResponse>> AddMessage([FromBody] AddMessageRequest request)
    {
        var user = await _usrRepository.Get(CurrentUserId);
        var chat = await _chatRepository.Get(CurrentChatId);
        if (user == null || (chat?.Members?.All(m => m != user) ?? true))
            return BadRequest(new GetChatResponse
            {
                Message = "Unknown chat"
            });

        var msg = await _msgRepository.Add(new Message 
        {
            Chat = chat,
            CreatedAt = DateTime.Now,
            Owner = user,
            Text = request.Text
        });

        return Ok(new AddMessageResponse
        {
            Result = true,
            Message = $"Message #{msg.Id} successfully added"
        });
    }

    [HttpDelete("{msgId:guid}")]
    public async Task<ActionResult<DeleteMessageResponse>> DeleteMessage(Guid msgId)
    {
        var user = await _usrRepository.Get(CurrentUserId);
        var chat = await _chatRepository.Get(CurrentChatId);
        if (chat?.Members?.All(m => m.Id != user?.Id) ?? true)
            return BadRequest(new DeleteMessageResponse
            {
                Message = "Unknown chat"
            });

        var msg = await _msgRepository.Get(msgId);
        if (msg == null || msg.Chat != chat)
            return BadRequest(new DeleteMessageResponse
            {
                Message = "Unknown message"
            });
        if (msg.Owner != user)
            return BadRequest(new DeleteMessageResponse
            {
                Message = "Access denied"
            });

        await _msgRepository.Remove(msg);
        return Ok(new DeleteMessageResponse
        {
            Result = true,
            Message = $"Message #{msg.Id} successfully deleted"
        });
    }
}