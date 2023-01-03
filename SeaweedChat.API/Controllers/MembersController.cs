using SeaweedChat.Domain.Aggregates;
using Microsoft.AspNetCore.Mvc;
using SeaweedChat.API.Models;
namespace SeaweedChat.API.Controllers;

[Route("api/v1/Chats/{ChatId:guid}/[controller]")]
public class MembersController : ApiController
{
    private readonly IMessageRepository _msgRepository;
    private readonly IChatRepository _chatRepository;
    public MembersController(
        IChatRepository chatRepository,
        IMessageRepository msgRepository,
        IUserRepository usrRepository,
        ILogger<MembersController> logger
    ) : base(logger, usrRepository)
    {
        _chatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
        _msgRepository = msgRepository ?? throw new ArgumentNullException(nameof(msgRepository));
    }

    public Task<Chat?> CurrentChat
    {
        get 
        {
            Guid.TryParse((string?)RouteData.Values.FirstOrDefault(v => v.Key == "ChatId").Value ?? "", out Guid chatId);
            return chatId == Guid.Empty
                ? Task.FromResult<Chat?>(null)
                : _chatRepository.Get(chatId);
        }
    }

    [HttpDelete("{memberId:guid}")]
    public async Task<ActionResult<DeleteMemberResponse>> DeleteMember([FromRoute] Guid memberId)
    {
        var chat = await CurrentChat;
        var currentMember = chat?.GetMemberByUser(CurrentUserId);
        if (chat == null || currentMember == null)
            return BadRequest(new DeleteMemberResponse
            {
                Message = "Unknown chat"
            });
        var member = chat.GetMemberByUser(memberId);
        if (member == null)
            return BadRequest(new DeleteMessageResponse
            {
               Message = "Unknown member" 
            });
        if (
            chat.Type == ChatType.Channel && 
            currentMember.Permission > ChatMemberPermission.Member &&
            currentMember.Permission > member.Permission
        )
            return BadRequest(new DeleteMessageResponse
            {
               Message = "Access denied" 
            });

        chat.RemoveMember(member);
        _logger?.LogInformation($"{member} has been added to {chat}");
        return Ok(new DeleteMessageResponse
        {
            Result = true,
            Message = $"Member #{member.Id} successfully deleted"
        });
    }
    [HttpPut]
    [ProducesResponseType(201)]
    public async Task<ActionResult<AddMemberResponse>> AddMember([FromBody] AddMemberRequest request)
    {
        var chat = await CurrentChat;
        var user = await (request.Username == null 
            ? _usrRepository.Get(request.UserId) 
            : _usrRepository.Get(request.Username)
        );
        var currentMember = chat?.GetMemberByUser(CurrentUserId);
        if (chat == null || currentMember == null)
            return BadRequest(new AddMemberResponse
            {
                Message = "Unknown chat"
            });
        if (user == null)
            return BadRequest(new AddMemberResponse
            {
                Message = "Unknown user"
            });
        if (chat.GetMemberByUser(user) != null)
            return BadRequest(new AddMemberResponse
            {
                Message = "User is already in chat"
            });
        if (
            chat.Type == ChatType.Channel && 
            currentMember.Permission <= ChatMemberPermission.Member
        )
        {
            return BadRequest(new AddMemberResponse
            {
                Message = "Access denied"
            });
        }

        var member = new ChatMember()
        {
            ChatId = chat.Id,
            Permission = ChatMemberPermission.Member,
            User = user
        };
        chat.AddMember(member);
        _logger?.LogInformation($"{member} has been added to chat {chat}");
        await _chatRepository.Update();

        return Created(
            CurrentRequestUri + $"/{member.Id}",
            new AddMemberResponse
            {
                Result = true,
                Message = $"Member #{member} successfully added"
            }
        );
    }
}