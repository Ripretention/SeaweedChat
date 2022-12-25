using System.ComponentModel.DataAnnotations;

namespace SeaweedChat.API.Models;
public class AddMessageRequest
{
    [Required]
    [MaxLength(4096)]
    public string Text { get; set; } = null!;
}