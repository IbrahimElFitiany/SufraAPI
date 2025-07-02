using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Utils;
using Sufra.Configuration;

public class EmailServices:IEmailServices
{
    private readonly SmtpSettings _smtpSettings;
    private readonly SupportSettings _supportSettings;

    public EmailServices(IOptionsSnapshot<SmtpSettings> smtpSettings, IOptionsSnapshot<SupportSettings> supportSettings)
    {
        _smtpSettings = smtpSettings.Value;
        _supportSettings = supportSettings.Value;
    }

    public async Task SendApprovalEmailAsync(string toEmail, string subject, string body, string base64)
    {
        string templatePath = Path.Combine(AppContext.BaseDirectory, "Infrastructure", "Services", "Templates", "ApprovedTemplate.html");
        string htmlTemplate = File.ReadAllText(templatePath);

        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_smtpSettings.FromName, _smtpSettings.FromEmail));
        emailMessage.To.Add(new MailboxAddress("", toEmail));                      
        emailMessage.Subject = subject;                                             

        var bodyBuilder = new BodyBuilder();

        var imageBytes = Convert.FromBase64String(base64);

        var image = bodyBuilder.LinkedResources.Add("reservation.png", imageBytes, new ContentType("image", "png"));
        image.ContentId = MimeUtils.GenerateMessageId();

        string finalHtml = htmlTemplate
            .Replace("{body}", body)
            .Replace("{imageCid}", image.ContentId);

        bodyBuilder.HtmlBody = finalHtml;
        bodyBuilder.TextBody = body;

        emailMessage.Body = bodyBuilder.ToMessageBody();

        try
        {
            using (var smtpClient = new SmtpClient())
            {
                await smtpClient.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, false);
                await smtpClient.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);

                await smtpClient.SendAsync(emailMessage);
                await smtpClient.DisconnectAsync(true);
            }

            Console.WriteLine("Email sent successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
            throw new Exception("An error occurred while sending email.", ex);
        }
    }
    public async Task SendRejectionEmailAsync(string toEmail, string subject)
    {
        string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Infrastructure", "Services", "Templates", "RejectionTemplate.html");
        string htmlTemplate = File.ReadAllText(templatePath);

        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_smtpSettings.FromName, _smtpSettings.FromEmail));
        emailMessage.To.Add(new MailboxAddress("", toEmail));
        emailMessage.Subject = subject;

        var bodyBuilder = new BodyBuilder();

        string rejectionMessage = "Unfortunately your reservation has been rejected. Please try again or contact our support team.";

        string finalHtml = htmlTemplate
            .Replace("{body}", rejectionMessage)
            .Replace("{supportEmail}", _supportSettings.Email)
            .Replace("{supportPhone}", _supportSettings.Phone);

        bodyBuilder.HtmlBody = finalHtml;
        bodyBuilder.TextBody = rejectionMessage;

        emailMessage.Body = bodyBuilder.ToMessageBody();

        try
        {
            using (var smtpClient = new SmtpClient())
            {
                await smtpClient.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, false);
                await smtpClient.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);

                await smtpClient.SendAsync(emailMessage);
                await smtpClient.DisconnectAsync(true);
            }

            Console.WriteLine("Rejection email sent successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending rejection email: {ex.Message}");
        }
    }
}
