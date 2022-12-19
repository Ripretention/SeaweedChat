using SeaweedChat.Domain.Aggregates;
namespace SeaweedChat.API.Models;

public class GetAllChatsResponse : Response
{
    public ICollection<Chat> Chats { get; set; } = new List<Chat>();
}