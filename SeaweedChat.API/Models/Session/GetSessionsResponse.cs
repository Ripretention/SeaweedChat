using SeaweedChat.Domain.Aggregates;
namespace SeaweedChat.API.Models;
public class GetSessionsResponse : Response
{
    public IEnumerable<Session> Sessions { get; set; } = Array.Empty<Session>();
}