using SeaweedChat.Domain.Aggregates;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
namespace SeaweedChat.Infra.Repositories;

public class ChatRepository : Repository, IChatRepository
{
    public ChatRepository(ApplicationContext context, ILogger<ChatRepository> logger)
        : base(context, logger)
    {}
    public async Task<ICollection<Chat>> GetAllUserChats(User? user)
    {
        _logger?.LogInformation($"get {user} chats");
        if (user == null)
            return Array.Empty<Chat>();
        return await _context.Chats
            .Include(c => c.Members)
            .Where(c => c.Members.Any(m => m.User == user))
            .ToArrayAsync();
    }

    public async Task<Chat?> Get(Guid id)
    {
        _logger?.LogInformation($"get chat by id <{id}>");
        return await _context.Chats
            .Include(c => c.Members)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> Remove(Chat chat)
    {
        _logger?.LogInformation($"remove {chat}");
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
        _logger?.LogInformation($"add {entity}");
        await _context.SaveChangesAsync();

        return entity;
    }
}