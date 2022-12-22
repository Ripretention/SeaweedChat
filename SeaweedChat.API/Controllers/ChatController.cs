using SeaweedChat.Domain.Aggregates;
using Microsoft.AspNetCore.Mvc;
using SeaweedChat.API.Models;
namespace SeaweedChat.API.Controllers;

[Route("api/[controller]s")]
public class ChatController : ApiController
{
    private readonly IChatRepository _chatRepository;
    public ChatController(
        IChatRepository chatRepository,
        IUserRepository usrRepository,
        ILogger<ChatController> logger
    ) : base(logger, usrRepository)
    {
        _chatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
    }

    [HttpGet("{ChatId:guid}")]
    public async Task<ActionResult<GetChatResponse>> GetChat(Guid chatId)
    {
        var user = await _usrRepository.Get(CurrentUserId);
        if (user == null)
            return BadRequest(new GetChatResponse
            {
                Message = "Unknown user"
            });
        var chat = await _chatRepository.GetUserChat(chatId, user);
        if (chat == null)
            return BadRequest(new GetChatResponse
            {
                Message = "Unknown chat"
            });

        return Ok(new GetChatResponse
        {
            Result = true,
            Message = "Success",
            Chat = chat
        });
    }

    [HttpGet]
    public async Task<ActionResult<GetAllChatsResponse>> GetAllChats()
    {
        var user = await _usrRepository.Get(CurrentUserId);
        if (user == null)
            return BadRequest(new GetAllChatsResponse
            {
                Message = "Unknown user"
            });

        return Ok(new GetAllChatsResponse
        {
            Result = true,
            Message = "Success",
            Chats = await _chatRepository.GetAllUserChats(user)
        });
    }

    [HttpPut]
    public async Task<ActionResult<AddChatResponse>> AddChat(AddChatRequest request)
    {
        var user = await _usrRepository.Get(CurrentUserId);
        if (user == null)
            return BadRequest(new AddChatResponse
            {
                Message = "Unknown user"
            });

        Chat chat;
        try 
        {
            chat = new Chat()
            {
                Title = request.Title,
                Type = request.Type
            };
            chat.AddMember(user);
            chat = await _chatRepository.Add(chat);
        }
        catch (Exception e)
        {
            _logger?.LogWarning($"Expection was caught at AddChat: {e.Message}");
            return BadRequest(new AddChatResponse
            {
                Message = e.Message
            });
        }

        return Ok(new AddChatResponse
        {
            Result = true,
            Message = $"Chat #{chat.Id} successfully added"
        });
    }
}