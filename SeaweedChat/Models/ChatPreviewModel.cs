namespace SeaweedChat.Models
{
    public class ChatPreviewModel
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public bool isSelected { get; set; }
        public System.TimeSpan LastMessage { get; set; }
    }
}
