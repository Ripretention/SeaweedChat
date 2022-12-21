namespace SeaweedChat.Domain.Aggregates;
public interface IMessageRepository : IRepository<Message>
{
    Task<IEnumerable<Message>> GetChatMessages(Chat chat, int offset, int limit);
}