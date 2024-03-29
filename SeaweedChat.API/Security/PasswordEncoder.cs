using System.Security.Cryptography;
using System.Text;
namespace SeaweedChat.API.Security;
public class PasswordEncoder : IPasswordEncoder
{
    private readonly byte[] _salt;
    public PasswordEncoder(string salt)
    {
        _salt = Encoding.UTF32.GetBytes(salt);
    }
    public string Encode(string password)
    {
        var passwordBytes = Encoding.UTF32.GetBytes(password);
        return Convert.ToBase64String(
            new Rfc2898DeriveBytes(passwordBytes, _salt, 6).GetBytes(24)
        );
    }
}