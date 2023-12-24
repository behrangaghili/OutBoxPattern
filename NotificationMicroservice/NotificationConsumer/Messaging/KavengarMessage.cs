namespace NotificationMicroservice.Messaging
{
    public class KavengarMessage : ITextMessageProvider
    {
        public void SendMessage(string message, string to)
        {
            Console.WriteLine($"message: {message} to {to}");
        }
    }
}
