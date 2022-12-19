using SeaweedChat.Domain.Aggregates;
namespace SeaweedChat.API.Models;

public class GetChatResponse : Response
{
    public Chat Chat { get; set; } = null!;
}