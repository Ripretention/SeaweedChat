namespace SeaweedChat.API.Models;

public class GetAllChatsResponse : Response
{
    public IEnumerable<ChatDto> Chats { get; set; } = Array.Empty<ChatDto>();
}