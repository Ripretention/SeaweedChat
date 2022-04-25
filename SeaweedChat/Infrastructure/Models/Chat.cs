using System;
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

        public IEnumerable<User> Members { get; set; }
        public IEnumerable<Message> Messages { get; set; }

        public bool Equals(Chat chat) => Id == chat.Id;
    }
}
