using SeaweedChat.Domain.Aggregates;
namespace SeaweedChat.API.Models;

public class GetMessageResponse : Response
{
    public Message? MessageBody { get; set; } = null!;
}