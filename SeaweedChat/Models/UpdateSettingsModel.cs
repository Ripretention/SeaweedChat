using System.ComponentModel.DataAnnotations;

namespace SeaweedChat.Models
{
    public class UpdateSettingsModel : RegisterModel
    {
        [Required(ErrorMessage = "Please enter confirm password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
