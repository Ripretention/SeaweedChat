using System.ComponentModel.DataAnnotations;

namespace SeaweedChat.API.Models;
public class EditMessageRequest
{
    [MaxLength(4096)]
    public string? Text { get; set; }
}