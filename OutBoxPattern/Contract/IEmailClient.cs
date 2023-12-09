namespace OutBoxPattern.Contract
{
    public interface IEmailClient
    {
        Task SendEmailAsync(string to, string subject, string body);
        // Additional methods for sending emails can be added as needed.
    }

}
