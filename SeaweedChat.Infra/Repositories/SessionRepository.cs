using SeaweedChat.Domain.Aggregates;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
namespace SeaweedChat.Infra.Repositories;

public class SessionRepository : Repository, ISessionRepository
{
    public SessionRepository(ApplicationContext context, ILogger<SessionRepository> logger)
        : base(context, logger)
    {}

    public async Task<bool> HasByAccountId(string token, Guid accountId)
    {
        _logger?.LogInformation($"check session of Account-{accountId} by token: <{string.Join("", token.Take(10))}>");
        return await _context.Sessions
            .Where(s => s.Account.Id == accountId)
            .AnyAsync(s => s.Token == token);
    }
    public async Task<ICollection<Session>> GetAllAccountSessions(Account? account)
    {
        _logger?.LogInformation($"get all session of {account}");
        if (account == null)
            return Array.Empty<Session>();
        return await _context.Sessions
            .Where(s => s.Account == account)
            .ToArrayAsync();
    }

    public async Task<Session?> Get(Guid id)
    {
        _logger?.LogInformation($"get session by id <{id}>");
        return await _context.Sessions.FindAsync(id);
    }

    public async Task<bool> Remove(Session msg)
    {
        _logger?.LogInformation($"remove {msg}");
        try 
        {
            _context.Sessions.Remove(msg);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
    public async Task<Session> Add(Session session)
    {
        var entity = (await _context.Sessions.AddAsync(session)).Entity;
        _logger?.LogInformation($"add {entity}");
        await _context.SaveChangesAsync();

        return entity;
    }
}