using SeaweedChat.Domain.Aggregates;

namespace SeaweedChat.API.Models;
public class AccountDto
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string Username { get; set; } = null!;
}