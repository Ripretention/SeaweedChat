namespace SeaweedChat.API.Models;
public class GetTokenRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}