using System.ComponentModel.DataAnnotations;
namespace SeaweedChat.API.Models;
public class EditAccountRequest
{
    [Required]
    [MinLength(6)]
    [MaxLength(128)]
    public string ConfirmationPassword { get; set; } = null!;
    [EmailAddress]
    public string? Email { get; set; }
    [MinLength(6)]
    [MaxLength(128)]
    public string? Password { get; set; }
    [MinLength(3)]
    [MaxLength(32)]
    [RegularExpression(@"^[a-zA-Z_\d]+$", ErrorMessage = "Username characters are not allowed.")]
    public string? Username { get; set; }
}