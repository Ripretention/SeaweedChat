namespace SeaweedChat.Domain.Aggregates;

public interface IPasswordEncoder
{
    public string Encode(string password);
}