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

    public Guid? CurrentChatId
    {
        get 
        {
            Guid id; 
            Guid.TryParse((string?)RouteData.Values.FirstOrDefault(v => v.Key == "ChatId").Value ?? "", out id);
            return id == Guid.Empty
                ? null
                : id;
        }
    }


    [HttpGet("{MsgId:Guid}")]
    public async Task<ActionResult<GetMessageResponse>> Get(Guid msgId)
    {
        var user = await _usrRepository.Get(CurrentUserId ?? Guid.Empty);
        if (user == null)
            return BadRequest(new GetMessageResponse
            {
                Message = "Unknown user"
            });
        var chat = await _chatRepository.GetUserChat(CurrentChatId ?? Guid.Empty, user);
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
        var user = await _usrRepository.Get(CurrentUserId ?? Guid.Empty);
        if (user == null)
            return BadRequest(new AddMessageResponse
            {
                Message = "Unknown user"
            });
        var chat = await _chatRepository.GetUserChat(CurrentChatId ?? Guid.Empty, user);
        if (chat == null)
            return BadRequest(new AddMessageResponse
            {
                Message = "Unknown chat"
            });

        var message = await _msgRepository.Add(new Message 
        {
            Chat = chat,
            Date = DateTime.Now,
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