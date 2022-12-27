using SeaweedChat.Domain.Aggregates;
namespace SeaweedChat.API.Models;

public class GetAllChatsResponse : Response
{
    public IEnumerable<Chat> Chats { get; set; } = new List<Chat>();
}