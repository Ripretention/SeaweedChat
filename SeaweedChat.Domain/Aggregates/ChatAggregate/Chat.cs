using System.Collections.Immutable;
namespace SeaweedChat.Domain.Aggregates;

public class Chat : Entity
{
    public ImmutableHashSet<User> Members { get => _members.ToImmutableHashSet(); }
    private HashSet<User> _members = new HashSet<User>();
    public ImmutableHashSet<Message> Messages { get => _messages.ToImmutableHashSet(); }
    private HashSet<Message> _messages = new HashSet<Message>();

    public bool AddMember(User usr) => 
        _members.Add(usr ?? throw new ArgumentNullException(nameof(usr)));
    public User GetUser(User usr) =>
        Members.First(u => u == usr);
    public bool RemoveUser(User usr) =>
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