using SeaweedChat.API.Models;
using Microsoft.AspNetCore.SignalR;
using SeaweedChat.Domain.Aggregates;
using SeaweedChat.API.Hubs.Abstractions;
namespace SeaweedChat.API.Hubs;

public class ChatHubContext : IChatHubContext
{
    private readonly ILogger<ChatHubContext>? _logger;
    private readonly IHubContext<ChatHub, IChatClient> _context;
    public ChatHubContext(
        IHubContext<ChatHub, IChatClient> context,
        ILogger<ChatHubContext>? logger
    )
    {
        _context = context;
        _logger = logger;
    }
    public async Task Send(Message message)
    {
        var messageDto = new MessageDto(message);
        try
        {
            var group = _context.Clients.Group($"chat {messageDto.ChatId}");
            await group.ReceiveMessage(messageDto);
        }
        catch (NullReferenceException)
        {
            _logger?.LogDebug($"User {message.Owner.Username} failed send to group <chat {messageDto.ChatId}>");
        }
    }
    public async Task Edit(Message message)
    {
        var messageDto = new MessageDto(message);
        try
        {
            await _context.Clients.Group($"chat {messageDto.ChatId}").ReceiveEditedMessage(messageDto);
        }
        catch (NullReferenceException)
        {
            _logger?.LogDebug($"User {message.Owner.Username} failed edit to group <chat {messageDto.ChatId}>");
        }
    }
    public async Task Delete(Message message)
    {
        try
        {
            await _context.Clients.Group($"chat {message.Chat.Id}").ReceiveDeletedMessage(message.Id);
        }
        catch (NullReferenceException)
        {
            _logger?.LogDebug($"User {message.Owner.Username} failed edit to group <chat {message.Chat.Id}>");
        }
    }
}