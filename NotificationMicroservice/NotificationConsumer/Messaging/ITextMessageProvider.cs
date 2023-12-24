namespace NotificationMicroservice.Messaging
{
    public interface ITextMessageProvider
    {
        void SendMessage(string message, string to);
    }
}
