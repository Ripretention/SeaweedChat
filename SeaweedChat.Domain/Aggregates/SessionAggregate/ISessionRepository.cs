namespace SeaweedChat.Domain.Aggregates;
public interface ISessionRepository : IRepository<Session>
{
    Task<bool> HasByAccountId(string token, Guid accountId);
    Task<ICollection<Session>> GetAllAccountSessions(Account? account);
}