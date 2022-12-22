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
        var user = await _usrRepository.Get(CurrentUserId);
        if (user == null)
            return BadRequest(new GetAllMessageResponse
            {
                Message = "Unknown user"
            });
        var chat = await _chatRepository.GetUserChat(CurrentChatId, user);
        if (chat == null)
            return BadRequest(new GetAllMessageResponse
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
        var user = await _usrRepository.Get(CurrentUserId);
        if (user == null)
            return BadRequest(new GetMessageResponse
            {
                Message = "Unknown user"
            });
        var chat = await _chatRepository.GetUserChat(CurrentChatId, user);
        if (chat == null)
            return BadRequest(new GetMessageResponse
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
        if (user == null)
            return BadRequest(new AddMessageResponse
            {
                Message = "Unknown user"
            });
        var chat = await _chatRepository.GetUserChat(CurrentChatId, user);
        if (chat == null)
            return BadRequest(new AddMessageResponse
            {
                Message = "Unknown chat"
            });

        var message = await _msgRepository.Add(new Message 
        {
            Chat = chat,
            CreatedAt = DateTime.Now,
            Owner = user,
            Text = request.Text
        });

        return Ok(new AddMessageResponse
        {
            Result = true,
            Message = $"Message #{message.Id} successfully added"
        });
    }
}