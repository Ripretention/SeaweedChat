using System.Collections.Immutable;
namespace SeaweedChat.Domain.Aggregates;

public class Chat : Entity
{
    public string Title { get; set; } = null!;
    public ChatType Type { get; set; } = ChatType.Dialoge;
    public IReadOnlyCollection<User> Members { get => _members.AsReadOnly(); }
    private List<User> _members = new List<User>();
    public ImmutableHashSet<Message> Messages { get => _messages.ToImmutableHashSet(); }
    private HashSet<Message> _messages = new HashSet<Message>();

    public bool AddMember(User usr)
    {
        if (_members.Contains(usr))
            return false;
        if (Type == ChatType.Dialoge && _members.Count >= 2)
            return false;

        _members.Add(usr ?? throw new ArgumentNullException(nameof(usr)));
        return true;
    }
    public User GetMember(User usr) =>
        Members.First(u => u == usr);
    public bool RemoveMember(User usr) =>
        _members.Remove(usr ?? throw new ArgumentNullException(nameof(usr)));

    public bool AddMessage(Message msg)
    {
        if (msg == null)
            throw new ArgumentNullException(nameof(msg));
        if (msg.Chat != this)
            throw new ArgumentException("Message doesn't belong the chat");

        return _messages.Add(msg);
    }
}