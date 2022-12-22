using SeaweedChat.Domain.Aggregates;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
namespace SeaweedChat.Infra.Repositories;

public class MessageRepository : Repository, IMessageRepository
{
    public MessageRepository(ApplicationContext context, ILogger<ChatRepository> logger)
        : base(context, logger)
    {}

    public async Task<Message?> Get(Guid id)
    {
        _logger?.LogInformation($"get message by id <{id}>");
        return await _context.Messages.FindAsync(id);
    }

    public async Task<IEnumerable<Message>> GetChatMessages(
        Chat chat, 
        int offset = 0, 
        int limit = 200
    )
    {        
        limit = Math.Abs(limit);
        _logger?.LogInformation($"get messages of {chat}, offset={offset}, limit={limit}");

        if (limit > 400)
            throw new ArgumentException("The limit maximum is 400");

        return await _context.Messages
            .AsNoTracking()
            .Where(m => m.Chat.Id == chat.Id)
            .OrderByDescending(c => c.CreatedAt)
            .Take(limit - Math.Abs(offset))
            .Include(m => m.Owner)
            .ToListAsync();
    }

    public async Task<bool> Remove(Message msg)
    {
        _logger?.LogInformation($"remove {msg}");
        try 
        {
            _context.Messages.Remove(msg);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
    public async Task<Message> Add(Message msg)
    {
        if (msg.Chat == null)
            throw new ArgumentNullException(nameof(msg.Chat));
        if (msg.Owner == null)
            throw new ArgumentNullException(nameof(msg.Owner));

        var entity = (await _context.Messages.AddAsync(msg)).Entity;
        _logger?.LogInformation($"add {entity} for {msg.Chat}");
        await _context.SaveChangesAsync();

        return entity;
    }
}