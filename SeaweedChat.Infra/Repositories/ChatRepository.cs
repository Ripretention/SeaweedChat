using SeaweedChat.Domain.Aggregates;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
namespace SeaweedChat.Infra.Repositories;

public class ChatRepository : IChatRepository
{
    private ApplicationContext _context;
    private ILogger<ChatRepository>? _logger;
    public ChatRepository(
        ApplicationContext context,
        ILogger<ChatRepository>? logger
    )
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger;
    }

    public async Task<ICollection<Chat>> GetAllUserChats(User user)
    {
        _logger?.LogInformation($"get all user #{user.Id} chats");
        return await _context.Chats
            .Include(c => c.Members)
            .Where(c => c.Members.Contains(user))
            .ToListAsync();
    }

    public async Task<Chat?> Get(Guid id)
    {
        _logger?.LogInformation($"get chat {id}");
        return await _context.Chats
            .Include(c => c.Members)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    public async Task<Chat?> GetUserChat(Guid id, User user)
    {
        _logger?.LogInformation($"get chat {id} of user #{user.Id} chats");
        return await _context.Chats
            .Include(c => c.Members)
            .Where(c => c.Members.Contains(user))
            .FirstAsync(c => c.Id == id);
    }

    public async Task<bool> Remove(Chat chat)
    {
        _logger?.LogInformation($"remove chat #{chat.Id}");
        try 
        {
            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
    public async Task<Chat> Add(Chat chat)
    {
        var entity = (await _context.Chats.AddAsync(chat)).Entity;
        _logger?.LogInformation($"add chat {entity.Id}");
        await _context.SaveChangesAsync();

        return entity;
    }
    public async Task<bool> Update()
    {
        _logger?.LogInformation($"update");
        return (await _context.SaveChangesAsync()) > 0;
    }
}