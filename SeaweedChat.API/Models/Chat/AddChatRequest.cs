using SeaweedChat.Domain.Aggregates;
using System.ComponentModel.DataAnnotations;
namespace SeaweedChat.API.Models;

public class AddChatRequest
{

    [Required]
    [MaxLength(128)]
    public string Title { get; set; } = null!;
    public ChatType Type { get; set; } = ChatType.Chat;
}