using SeaweedChat.Domain.Aggregates;
namespace SeaweedChat.API.Models;

public class GetAllMessageResponse : Response
{
    public IEnumerable<Message> Messages { get; set; } = new List<Message>();
}