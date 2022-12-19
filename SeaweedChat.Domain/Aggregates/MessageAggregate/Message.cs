namespace SeaweedChat.Domain.Aggregates;

public class Message : Entity
{
    public DateTime Date { get; set; }
    public string? Text { get; set; }
    public User Owner { get; set; } = null!;
    public Chat Chat { get; set; } = null!;
}