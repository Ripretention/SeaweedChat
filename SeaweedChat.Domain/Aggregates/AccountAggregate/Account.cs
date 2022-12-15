using System.Security.Cryptography;
namespace SeaweedChat.Domain.Aggregates;

public class Account : Entity
{
    public string Email { get; set; }
    public string Password 
    {
        get => _password;
        set
        {
            _password = _encoder.Encode(value);
        }
    }
    public User User { get; set; }
    private string _password;

    private readonly PasswordEncoder _encoder;
    public Account(PasswordEncoder encoder)
    {
        _encoder = encoder;
    }

    public bool VerifyPassword(string password) =>
        Password == _encoder.Encode(password);
}