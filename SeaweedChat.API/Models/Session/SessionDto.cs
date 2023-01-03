namespace SeaweedChat.API.Models;
public class SessionDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Token { get; set; } = null!;
}