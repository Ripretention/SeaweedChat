namespace SeaweedChat.API.Security;
public interface IPasswordEncoder
{
    string Encode(string password);
}