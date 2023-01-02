namespace SeaweedChat.Domain.Aggregates;

public interface IUserRepository : IRepository<User>
{
    Task<User?> Get(string username);
    Task<bool> HasUser(string username);
}