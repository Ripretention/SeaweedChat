namespace SeaweedChat.Domain.Aggregates;

public interface IChatRepository : IRepository<Chat>
{
    Task<ICollection<Chat>> GetAllUserChats(User? user);
}