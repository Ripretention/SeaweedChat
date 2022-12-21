using SeaweedChat.Domain.Aggregates;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
namespace SeaweedChat.Infra.Repositories;

public class MessageRepository : IMessageRepository
{
    private ApplicationContext _context;
    private ILogger<MessageRepository>? _logger;
    public MessageRepository(
        ApplicationContext context,
        ILogger<MessageRepository>? logger
    )
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger;
    }

    public async Task<Message?> Get(Guid id)
    {
        _logger?.LogInformation($"get message {id}");
        return await _context.Messages.FindAsync(id);
    }

    public async Task<IEnumerable<Message>> GetChatMessages(
        Chat chat, 
        int offset = 0, 
        int limit = 200
    )
    {
        limit = Math.Abs(limit);
        if (limit > 400)
            throw new ArgumentException("The limit maximum is 400");

        return await _context.Messages
            .Where(c => c.Id == chat.Id)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<bool> Remove(Message msg)
    {
        _logger?.LogInformation($"remove message #{msg.Id}");
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
        _logger?.LogInformation($"add message {entity.Id}");
        await _context.SaveChangesAsync();

        return entity;
    }
    public async Task<bool> Update()
    {
        _logger?.LogInformation($"update");
        return (await _context.SaveChangesAsync()) > 0;
    }
}