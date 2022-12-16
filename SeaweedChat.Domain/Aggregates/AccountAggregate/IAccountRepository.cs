namespace SeaweedChat.Domain.Aggregates;

public interface IAccountRepository : IRepository<Account>
{
    Task<Account?> GetByEmail(string email);
    Task<bool> HasAccount(string email);
}