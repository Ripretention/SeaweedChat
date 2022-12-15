namespace SeaweedChat.Domain.Aggregates;

public class Message : Entity
{
    public DateTime Date { get; private set; }
    public string? Text { get; private set; }
    public User Owner { get; private set; } = null!;
    public Chat Chat { get; private set; } = null!;
}