using System.Security.Cryptography;
namespace SeaweedChat.Domain.Aggregates;

public class Account : Entity
{
    public string Email { get; set; } = null!;
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
        }
    }
    public User User { get; set; } = null!;
    private string _password = null!;

    public bool VerifyPassword(string password) =>
        Password == password;
}