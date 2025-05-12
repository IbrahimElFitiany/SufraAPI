
    public interface IEmailServices
    {
        Task SendEmailAsync(string toEmail, string subject, string body, string base64);
        Task SendRejectionEmailAsync(string toEmail, string subject);
    }

