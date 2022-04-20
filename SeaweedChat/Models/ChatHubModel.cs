using System.Collections.Generic;

namespace SeaweedChat.Models
{
    public class ChatHubModel
    {
        public int SelectedChatId { get; set; }
        public List<Infrastructure.Models.Message> Messages;
    }
}
