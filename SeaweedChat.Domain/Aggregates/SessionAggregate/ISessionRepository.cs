namespace SeaweedChat.Domain.Aggregates;
public interface ISessionRepository : IRepository<Session>
{
    Task<ICollection<Session>> GetAllAccountSessions(Account? account);
}