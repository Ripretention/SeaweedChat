namespace SeaweedChat.Domain.Aggregates;

public class Message : Entity
{
    public DateTime Date { get; private set; }
    public string? Text { get; private set; }
    public User Owner { get; private set; }
    public Chat Chat { get; private set; }

    public Message(
        Chat chat,
        User usr,
        string? text
    ) {
        Chat = chat;
        Owner = usr;
        Text = text;
    }
}