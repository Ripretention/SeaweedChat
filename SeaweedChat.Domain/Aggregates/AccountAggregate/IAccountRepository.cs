namespace SeaweedChat.Domain.Aggregates;

public interface IAccountRepository : IRepository<Account>
{
    public bool HasAccount(string email);
}