using Microsoft.Extensions.Logging;
namespace SeaweedChat.Infra.Repositories;
public class Repository
{
    protected ApplicationContext _context;
    protected ILogger? _logger;
    public Repository(
        ApplicationContext context,
        ILogger? logger
    )
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger;
    }

    public async Task<bool> Update()
    {
        _logger?.LogDebug($"update");
        return (await _context.SaveChangesAsync()) > 0;
    }
}