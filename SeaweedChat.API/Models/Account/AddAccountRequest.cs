using System.ComponentModel.DataAnnotations;

namespace SeaweedChat.API.Models;
public class AddAccountRequest
{
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
    [Required]
    public string Username { get; set; } = null!;
}