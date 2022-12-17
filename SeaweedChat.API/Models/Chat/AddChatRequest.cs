using SeaweedChat.Domain.Aggregates;
using System.ComponentModel.DataAnnotations;
namespace SeaweedChat.API.Models;

public class AddChatRequest
{

    [Required]
    public string Title { get; set; } = null!;
    [Required]
    public ChatType Type { get; set; } = ChatType.Dialoge;
}