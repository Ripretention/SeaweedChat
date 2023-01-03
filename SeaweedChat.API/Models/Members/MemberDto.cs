using SeaweedChat.Domain.Aggregates;

namespace SeaweedChat.API.Models;
public class MemberDto
{
    public Guid Id { get; set; }
    public Guid ChatId { get; set; }
    public string Username { get; set; } = null!;
    public ChatMemberPermission Permission { get; set; }
    public DateTime LastActivityAt { get; set; }
}