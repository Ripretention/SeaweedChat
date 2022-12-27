namespace SeaweedChat.Domain.Aggregates;
public interface IMessageRepository : IRepository<Message>
{
    Task<ICollection<Message>> GetChatMessages(Chat? chat, int offset, int limit);
}