using SeaweedChat.Domain.Aggregates;
using Microsoft.AspNetCore.Mvc;
using SeaweedChat.API.Models;
namespace SeaweedChat.API.Controllers;

[Route("api/v1/[controller]")]
public class ChatsController : ApiController
{
    private readonly IChatRepository _chatRepository;
    public ChatsController(
        IChatRepository chatRepository,
        IUserRepository usrRepository,
        ILogger<ChatsController> logger
    ) : base(logger, usrRepository)
    {
        _chatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
    }

    [HttpGet("{chatId:guid}")]
    public async Task<ActionResult<GetChatResponse>> GetChat([FromRoute] Guid chatId)
    {
        var chat = await _chatRepository.Get(chatId);
        if (chat?.GetMemberByUser(CurrentUserId) == null)
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
    [ProducesResponseType(201)]
    public async Task<ActionResult<AddChatResponse>> AddChat([FromBody] AddChatRequest request)
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
            chat.AddMember(new ChatMember
            {
                ChatId = chat.Id,
                User = user,
                Permission = ChatMemberPermission.Owner
            });
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

        return Created(
            CurrentRequestUri + $"/{chat.Id}",
            new AddChatResponse
            {
                Result = true,
                Message = $"Chat #{chat.Id} successfully added"
            }
        );
    }
    [HttpDelete("{chatId:guid}")]
    public async Task<ActionResult<DeleteChatResponse>> DeleteChat([FromRoute] Guid chatId)
    {
        var chat = await _chatRepository.Get(chatId);
        if (chat?.GetMemberByUser(CurrentUserId) == null)
            return BadRequest(new DeleteChatResponse
            {
                Message = "Unknown chat"
            });
        
        if (chat.Type == ChatType.Channel)
        {
            var member = chat.Members.First(m => m.User.Id == CurrentUserId);
            if (member?.Permission == ChatMemberPermission.Member)
                return BadRequest(new DeleteChatResponse
                {
                   Message = "Access denied" 
                });
        }

        await _chatRepository.Remove(chat);
        return Ok(new DeleteChatResponse
        {
            Result = true,
            Message = "Success"
        });
    }
}