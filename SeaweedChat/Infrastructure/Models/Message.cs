using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeaweedChat.Infrastructure.Models
{
    public class Message
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; private set; }
        public User Owner { get; set; }
        public User Peer { get; set; }
        public string Text { get; set; }
        public bool isReaded { get; set; }
        public System.DateTime Date { get; set; }
    }
}
