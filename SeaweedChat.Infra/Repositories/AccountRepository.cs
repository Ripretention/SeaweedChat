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
        return await _context.Accounts.FindAsync(id);
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
        _logger?.LogInformation($"add account {account.Id}");
        var entity = (await _context.Accounts.AddAsync(account)).Entity;
        await _context.SaveChangesAsync();

        return entity;
    }
    public async Task<bool> Update()
    {
        _logger?.LogInformation($"update");
        return (await _context.SaveChangesAsync()) > 0;
    }
}