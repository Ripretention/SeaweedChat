using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeaweedChat.Infrastructure.Models
{
    public class Chat : IEquatable<Chat>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; private set; }
        public virtual ICollection<User> Members { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
        public bool Equals(Chat chat) => Id == chat.Id;
    }
}
