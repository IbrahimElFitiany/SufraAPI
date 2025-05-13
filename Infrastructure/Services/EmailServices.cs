using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Utils;

public class EmailServices:IEmailServices
{

    public async Task SendApprovalEmailAsync(string toEmail, string subject, string body, string base64)
    {
        string templatePath = Path.Combine(Directory.GetCurrentDirectory(),"Infrastructure" , "Services" , "Templates", "ApprovedTemplate.html");
        string htmlTemplate = File.ReadAllText(templatePath);

        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Sufra", "sufraa.app@gmail.com"));
        emailMessage.To.Add(new MailboxAddress("", toEmail));                      
        emailMessage.Subject = subject;                                             

        var bodyBuilder = new BodyBuilder();

        var imageBytes = Convert.FromBase64String(base64);

        // 5. Create a linked resource (embedded image) and get its CID
        var image = bodyBuilder.LinkedResources.Add("reservation.png", imageBytes, new ContentType("image", "png"));
        image.ContentId = MimeUtils.GenerateMessageId();

        string finalHtml = htmlTemplate
            .Replace("{body}", body)
            .Replace("{imageCid}", image.ContentId);

        // 7. Set the body content
        bodyBuilder.HtmlBody = finalHtml;
        bodyBuilder.TextBody = body;

        emailMessage.Body = bodyBuilder.ToMessageBody();

        try
        {
            using (var smtpClient = new SmtpClient())
            {
                await smtpClient.ConnectAsync("smtp.gmail.com", 587, false); // Connect to Gmail SMTP server
                await smtpClient.AuthenticateAsync("sufraa.app@gmail.com", "gtdi kqyt xvet pnvy");

                await smtpClient.SendAsync(emailMessage);
                await smtpClient.DisconnectAsync(true);
            }

            Console.WriteLine("Email sent successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
        }
    }
    public async Task SendRejectionEmailAsync(string toEmail, string subject)
    {
        string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Infrastructure", "Services", "Templates", "RejectionTemplate.html");
        string htmlTemplate = File.ReadAllText(templatePath);

        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Sufra", "sufraa.app@gmail.com"));
        emailMessage.To.Add(new MailboxAddress("", toEmail));
        emailMessage.Subject = subject;

        var bodyBuilder = new BodyBuilder();

        string rejectionMessage = "Unfortunately your reservation has been rejected. Please try again or contact our support team.";

        string finalHtml = htmlTemplate
            .Replace("{body}", rejectionMessage)
            .Replace("{supportEmail}", "support@sufra.com")
            .Replace("{supportPhone}", "+1 (123) 456-7890");

        bodyBuilder.HtmlBody = finalHtml;
        bodyBuilder.TextBody = rejectionMessage;

        emailMessage.Body = bodyBuilder.ToMessageBody();

        try
        {
            using (var smtpClient = new SmtpClient())
            {
                await smtpClient.ConnectAsync("smtp.gmail.com", 587, false);
                await smtpClient.AuthenticateAsync("sufraa.app@gmail.com", "gtdi kqyt xvet pnvy");

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
