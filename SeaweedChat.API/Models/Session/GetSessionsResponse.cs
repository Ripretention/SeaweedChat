namespace SeaweedChat.API.Models;
public class GetSessionsResponse : Response
{
    public IEnumerable<SessionDto> Sessions { get; set; } = Array.Empty<SessionDto>();
}