using SeaweedChat.API.Models;
namespace SeaweedChat.API.Hubs.Abstractions;

public interface IChatClient
{
    Task Subscribe(bool result);
    Task ReceiveMessage(MessageDto message);
    Task ReceiveEditedMessage(MessageDto newMessage);
    Task ReceiveDeletedMessage(Guid deletedMessageId);
}