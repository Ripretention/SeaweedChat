using SeaweedChat.API.Models;
using SeaweedChat.Domain.Aggregates;
using SeaweedChat.API.Hubs.Abstractions;
namespace SeaweedChat.API.Hubs;

public class ChatHub : ApiHub<IChatClient>
{
    private readonly IChatRepository _chatRepository;
    public ChatHub(
        IUserRepository usrRepository,
        IChatRepository chatRepository,
        ILogger<ChatHub> logger
    ) : base(usrRepository, logger)
    {
        _chatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
    }
    
    public async Task Subscribe(string rawChatId)
    {
        Guid.TryParse(rawChatId, out Guid chatId);

        var user = await _usrRepository.Get(CurrentUserId);
        if (user == null)
        {
            await Clients.Caller.Subscribe(false);
            return;
        }

        var chats = await _chatRepository.GetAllUserChats(user);
        var hasChat = chats.Any(chat => chat.Id == chatId);
        if (hasChat)
            await Groups.AddToGroupAsync(Context.ConnectionId, $"chat {chatId}");

        await Clients.Caller.Subscribe(hasChat);
        _logger?.LogInformation($"User {user.Username} subscribed on chat {chatId} [{(hasChat ? "SUCCESS" : "FAILURE")}]");
    }
}