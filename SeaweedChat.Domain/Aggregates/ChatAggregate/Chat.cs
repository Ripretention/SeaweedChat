namespace SeaweedChat.Domain.Aggregates;

public class Chat : Entity
{
    public string Title { get; set; } = null!;
    public IReadOnlyCollection<User> Members { get => _members.AsReadOnly(); }
    protected List<User> _members = new List<User>();

    public bool AddMember(User usr)
    {
        if (_members.Contains(usr))
            return false;

        _members.Add(usr ?? throw new ArgumentNullException(nameof(usr)));
        return true;
    }
    public User? GetMember(User usr) => GetMember(usr.Id);
    public User? GetMember(Guid usrId) =>
        Members.FirstOrDefault(u => u.Id == usrId);
    public bool RemoveMember(User usr) =>
        _members.Remove(usr ?? throw new ArgumentNullException(nameof(usr)));
}