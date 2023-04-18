using SeaweedChat.Domain.Aggregates;
namespace SeaweedChat.API.Models;

public class MessageDto
{
    public MessageDto(Message msg)
    {
        Id = msg.Id;
        Text = msg.Text;
        ChatId = msg.Chat.Id;
        OwnerId = msg.Owner.Id;
        OwnerUsername = msg.Owner.Username;
        CreatedAt = msg.CreatedAt;
        EditAt = msg.EditAt;
    }
    public Guid Id { get; set; }
    public string? Text { get; set; }
    public Guid OwnerId { get; set; }
    public Guid ChatId { get; set; }
    public string OwnerUsername { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? EditAt { get; set; } = null;
}