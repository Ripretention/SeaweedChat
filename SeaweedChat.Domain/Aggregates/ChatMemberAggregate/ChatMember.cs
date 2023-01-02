namespace SeaweedChat.Domain.Aggregates;
public class ChatMember : Entity
{
    public Guid ChatId { get; set; }
    public User User { get; set; } = null!;
    public ChatMemberPermission Permission { get; set; } = ChatMemberPermission.Member;
    public DateTime LastActivityAt { get; set; } = DateTime.Now;
}