using System.ComponentModel.DataAnnotations;
namespace SeaweedChat.API.Models;
public class AddMemberRequest
{
    public Guid UserId { get; set; }
    public string? Username { get; set; }
}