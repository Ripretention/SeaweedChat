namespace SeaweedChat.Domain.Aggregates;

public class Chat : Entity
{
    public string Title { get; set; } = null!;
    public ChatType Type { get; set; } = ChatType.Chat;
    public IReadOnlyCollection<ChatMember> Members { get => _members.AsReadOnly(); }
    protected List<ChatMember> _members = new List<ChatMember>();

    public bool AddMember(ChatMember member)
    {
        if (_members.Contains(member))
            return false;

        _members.Add(member ?? throw new ArgumentNullException(nameof(member)));
        return true;
    }
    public ChatMember? GetMemberByUser(User usr) =>
        GetMemberByUser(usr.Id);
    public ChatMember? GetMemberByUser(Guid usrId) =>
        Members.FirstOrDefault(u => u.User.Id == usrId);
    public ChatMember? GetMember(ChatMember member) => 
        GetMember(member.Id);
    public ChatMember? GetMember(Guid memberId) =>
        Members.FirstOrDefault(u => u.Id == memberId);
    public bool RemoveMember(ChatMember member) =>
        _members.Remove(member ?? throw new ArgumentNullException(nameof(member)));
}