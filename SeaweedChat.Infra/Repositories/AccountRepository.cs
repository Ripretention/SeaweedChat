using Microsoft.EntityFrameworkCore;
using SeaweedChat.Domain.Aggregates;
using Microsoft.Extensions.Logging;
namespace SeaweedChat.Infra.Repositories;

public class AccountRepository : IAccountRepository
{
    private ApplicationContext _context;
    private ILogger<AccountRepository>? _logger;
    public AccountRepository(
        ApplicationContext context,
        ILogger<AccountRepository>? logger
    )
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger;
    }

    public async Task<Account?> Get(Guid id)
    {
        _logger?.LogInformation($"get account {id}");
        return await _context.Accounts
            .Include(u => u.User)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Account?> GetByEmail(string email)
    {
        _logger?.LogInformation($"get account by email <{email}>");
        return await _context.Accounts
            .Include(u => u.User)
            .FirstOrDefaultAsync(a => a.Email == email);
    }

    public async Task<bool> Remove(Account account)
    {
        _logger?.LogInformation($"remove account #{account.Id}");
        try 
        {
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
    public async Task<Account> Add(Account account)
    {
        if (await HasAccount(account.Email))
            throw new ArgumentException("Accout with such email already exist");

        var entity = (await _context.Accounts.AddAsync(account)).Entity;
        _logger?.LogInformation($"add account {entity.Id}");
        await _context.SaveChangesAsync();

        return entity;
    }
    public async Task<bool> HasAccount(string email)
    {
        _logger?.LogInformation($"looking for account by email <{email}>");
        return email != null && await _context.Accounts.AnyAsync(acc => acc.Email == email);
    }
    public async Task<bool> Update()
    {
        _logger?.LogInformation($"update");
        return (await _context.SaveChangesAsync()) > 0;
    }
}