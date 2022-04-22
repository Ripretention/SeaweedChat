using System.Collections.Generic;
using SeaweedChat.Infrastructure.Models;

namespace SeaweedChat.Models
{
    public class ChatModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<User> Members { get; set; }
        public IEnumerable<Message> Messages { get; set; }
        public IEnumerable<ChatPreviewModel> ChatPreviews { get; set; }
    }
}
