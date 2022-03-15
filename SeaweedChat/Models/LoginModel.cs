using System.ComponentModel.DataAnnotations;

namespace SeaweedChat.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter nickname")]
        [StringLength(32, ErrorMessage = "Max length for nickname = 32chars")]
        [RegularExpression(@"[a-zA-Z0-9_]+", ErrorMessage = "Username containse invalid symbols")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
