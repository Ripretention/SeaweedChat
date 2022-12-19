using System.ComponentModel.DataAnnotations;

namespace SeaweedChat.API.Models;
public class AddMessageRequest
{
    [Required]
    public string Text { get; set; } = null!;
}