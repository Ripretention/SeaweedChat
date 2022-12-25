using System.ComponentModel.DataAnnotations;

namespace SeaweedChat.API.Models;
public class AddAccountRequest
{
    [EmailAddress]
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    [MinLength(6)]
    [MaxLength(128)]
    public string Password { get; set; } = null!;
    [Required]
    [MinLength(3)]
    [MaxLength(32)]
    [RegularExpression(@"^[a-zA-Z_\d]+$", ErrorMessage = "Username characters are not allowed.")]
    public string Username { get; set; } = null!;
}