namespace SeaweedChat.Domain.Aggregates;

public class Session : Entity
{
    public Account Account { get; set; } = null!;
    public DateTime Date { get; set; }
    public string Token { get; set; } = null!;
}