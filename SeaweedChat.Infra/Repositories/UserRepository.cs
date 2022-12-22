using SeaweedChat.Domain.Aggregates;
using Microsoft.Extensions.Logging;
namespace SeaweedChat.Infra.Repositories;

public class UserRepository : Repository, IUserRepository
{
    public UserRepository(ApplicationContext context, ILogger<ChatRepository> logger)
        : base(context, logger)
    {}

    public async Task<User?> Get(Guid id)
    {
        _logger?.LogInformation($"get user by id <{id}>");
        return await _context.Users.FindAsync(id);
    }

    public async Task<bool> Remove(User usr)
    {
        _logger?.LogInformation($"remove {usr}");
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

    public bool HasUser(string username)
    {
        _logger?.LogInformation($"looking for user by username <{username}>");
        return username != null && _context.Users.Any(acc => acc.Username == username);
    }
    public async Task<User> Add(User usr)
    {
        if (HasUser(usr.Username))
            throw new ArgumentException("User with such username already exist");

        var entity = (await _context.Users.AddAsync(usr)).Entity;
         _logger?.LogInformation($"add {entity}");
        await _context.SaveChangesAsync();

        return entity;
    }
}