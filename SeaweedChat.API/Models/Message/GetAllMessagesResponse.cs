namespace SeaweedChat.API.Models;
public class GetAllMessageResponse : Response
{
    public IEnumerable<MessageDto> Messages { get; set; } = Array.Empty<MessageDto>();
}