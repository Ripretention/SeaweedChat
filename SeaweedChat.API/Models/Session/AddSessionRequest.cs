using System.ComponentModel.DataAnnotations;

namespace SeaweedChat.API.Models;
public class AddSessionRequest
{
    [EmailAddress]
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    [MinLength(6)]
    [MaxLength(128)]
    public string Password { get; set; } = null!;
}