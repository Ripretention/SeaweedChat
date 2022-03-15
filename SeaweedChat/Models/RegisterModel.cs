using System.ComponentModel.DataAnnotations;

namespace SeaweedChat.Models
{
    public class RegisterModel : LoginModel
    {
        [Required(ErrorMessage = "Please enter email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
