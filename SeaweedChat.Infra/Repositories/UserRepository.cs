using SeaweedChat.Domain.Aggregates;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
namespace SeaweedChat.Infra.Repositories;

public class UserRepository : Repository, IUserRepository
{
    public UserRepository(ApplicationContext context, ILogger<ChatRepository> logger)
        : base(context, logger)
    {}

    public async Task<User?> Get(string username)
    {
        _logger?.LogDebug($"get user by username <{username}>");
        return await _context.Users.FirstOrDefaultAsync(usr => usr.Username == username);
    }
    public async Task<User?> Get(Guid id)
    {
        _logger?.LogDebug($"get user by id <{id}>");
        return await _context.Users.FindAsync(id);
    }

    public async Task<bool> Remove(User usr)
    {
        _logger?.LogDebug($"remove {usr}");
        try 
        {
            _context.Users.Remove(usr);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> HasUser(string username)
    {
        _logger?.LogDebug($"looking for user by username <{username}>");
        return username != null && await _context.Users.AnyAsync(acc => acc.Username == username);
    }
    public async Task<User> Add(User usr)
    {
        if (await HasUser(usr.Username))
            throw new ArgumentException("User with such username already exist");

        var entity = (await _context.Users.AddAsync(usr)).Entity;
         _logger?.LogDebug($"add {entity}");
        await _context.SaveChangesAsync();

        return entity;
    }
}