namespace SeaweedChat.API.Models;
public class AddAccountRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Username { get; set; } = null!;
}