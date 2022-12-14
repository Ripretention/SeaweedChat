using SeaweedChat.Domain.Aggregates;
using Microsoft.Extensions.Logging;
namespace SeaweedChat.Infra.Repositories;

public class UserRepository : IUserRepository
{
    private ApplicationContext _context;
    private ILogger<UserRepository>? _logger;
    public UserRepository(
        ApplicationContext context,
        ILogger<UserRepository>? logger
    )
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger;
    }

    public async Task<User?> Get(Guid id)
    {
        _logger?.LogInformation($"get user {id}");
        return await _context.Users.FindAsync(id);
    }

    public async Task<bool> Remove(User usr)
    {
        _logger?.LogInformation($"remove user #{usr.Id}");
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
    public async Task<User> Add(User usr)
    {
        _logger?.LogInformation($"add user {usr.Id}");
        var entity = (await _context.Users.AddAsync(usr)).Entity;
        await _context.SaveChangesAsync();

        return entity;
    }
    public async Task<bool> Update()
    {
        _logger?.LogInformation($"update");
        return (await _context.SaveChangesAsync()) > 0;
    }
}