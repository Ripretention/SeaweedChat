namespace SeaweedChat.Domain.Aggregates;

public interface IUserRepository : IRepository<User>
{
    public bool HasUser(string username);
}