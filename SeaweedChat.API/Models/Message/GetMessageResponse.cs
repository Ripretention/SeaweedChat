namespace SeaweedChat.API.Models;

public class GetMessageResponse : Response
{
    public MessageDto? MessageBody { get; set; } = null!;
}