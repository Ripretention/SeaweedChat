using SeaweedChat.Domain.Aggregates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SeaweedChat.API.Models;
namespace SeaweedChat.API.Controllers;

public class ChatController : ApiController
{
    private readonly ILogger<ChatController>? _logger;
    private readonly IUserRepository _usrRepository;
    private readonly IChatRepository _chatRepository;
    public ChatController(
        IUserRepository usrRepository,
        IChatRepository chatRepository,
        ILogger<ChatController> logger
    )
    {
        _usrRepository = usrRepository ?? throw new ArgumentNullException(nameof(usrRepository));
        _chatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<GetAllResponse>> GetAll()
    {
        var user = await _usrRepository.Get(CurrentUserId ?? Guid.Empty);
        if (user == null)
            return BadRequest(new AddChatResponse
            {
                Message = "Unknown user"
            });

        return Ok(new GetAllResponse
        {
            Result = true,
            Chats = await _chatRepository.GetAllByUser(user)
        });
    }

    [HttpPut]
    public async Task<ActionResult<AddChatResponse>> AddChat(AddChatRequest request)
    {
        var user = await _usrRepository.Get(CurrentUserId ?? Guid.Empty);
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