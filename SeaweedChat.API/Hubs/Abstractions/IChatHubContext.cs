using SeaweedChat.Domain.Aggregates;
namespace SeaweedChat.API.Hubs.Abstractions;

public interface IChatHubContext
{
    Task Send(Message message);
    Task Edit(Message message);
    Task Delete(Message message);
}