namespace SeaweedChat.API.Models;

public class GetChatResponse : Response
{
    public ChatDto Chat { get; set; } = null!;
}