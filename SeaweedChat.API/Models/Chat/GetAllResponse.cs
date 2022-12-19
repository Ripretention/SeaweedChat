using SeaweedChat.Domain.Aggregates;
namespace SeaweedChat.API.Models;

public class GetAllResponse : Response
{
    public ICollection<Chat> Chats { get; set; } = new List<Chat>();
}