using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SeaweedChat.Infrastructure.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [MaxLength(32)]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        [MaxLength(256, ErrorMessage = "Max length for password is 256")]
        public string Password { get; set; }
    }
}
