namespace SeaweedChat.Domain.Aggregates;

public class Message : Entity
{
    public DateTime CreatedAt { get; set; }
    public DateTime? EditAt { get; set; } = null;
    public string? Text { get; set; }
    public User Owner { get; set; } = null!;
    public Chat Chat { get; set; } = null!;
}