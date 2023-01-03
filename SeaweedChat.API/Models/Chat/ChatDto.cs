using SeaweedChat.Domain.Aggregates;

namespace SeaweedChat.API.Models;
public class ChatDto
{
    public ChatDto(Chat chat)
    {
        Id = chat.Id;
        Title = chat.Title;
        Type = chat.Type;
        Members = chat.Members.Select(mbr => new MemberDto
        {
            Id = mbr.Id,
            ChatId = mbr.Chat.Id,
            Permission = mbr.Permission,
            Username = mbr.User.Username,
            LastActivityAt = mbr.LastActivityAt
        });
    }
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public ChatType Type { get; set; }
    public IEnumerable<MemberDto> Members { get; set; }
}