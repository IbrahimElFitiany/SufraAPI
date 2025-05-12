using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Utils;

public class EmailServices:IEmailServices
{
    public async Task SendEmailAsync(string toEmail, string subject, string body, string base64)
    {
        var emailMessage = new MimeMessage();

        // Set sender's email address
        emailMessage.From.Add(new MailboxAddress("Sufra", "sufraa.app@gmail.com"));

        // Set recipient's email address
        emailMessage.To.Add(new MailboxAddress("", toEmail));

        // Set subject
        emailMessage.Subject = subject;

        var bodyBuilder = new BodyBuilder();

        // Convert base64 string to byte array
        var imageBytes = Convert.FromBase64String(base64);

        // Create a linked resource (embedded image)
        var image = bodyBuilder.LinkedResources.Add("reservation.png", imageBytes, new ContentType("image", "png"));
        image.ContentId = MimeUtils.GenerateMessageId();

        // Set both text and HTML bodies
        bodyBuilder.TextBody = body;
        bodyBuilder.HtmlBody = $@"
                <html>
                    <head>
                        <style>
                            body {{
                                font-family: 'Arial', sans-serif;
                                margin: 0;
                                padding: 0;
                                background-color: #f8f9fa;
                                color: #333;
                            }}
                            .email-container {{
                                max-width: 600px;
                                margin: 0 auto;
                                padding: 20px;
                            }}
                            .content {{
                                background-color: #ffffff;
                                padding: 30px;
                                border-radius: 12px;
                                box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
                                text-align: center;
                            }}
                            .header {{
                                margin-bottom: 25px;
                            }}
                            h1 {{
                                color: #2c3e50;
                                font-size: 24px;
                                margin-bottom: 20px;
                            }}
                            .image-container {{
                                margin: 20px 0;
                            }}
                            .reservation-image {{
                                max-width: 300px;
                                width: 100%;
                                height: auto;
                                border-radius: 8px;
                                border: 1px solid #eee;
                                box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
                                display: block;
                                margin: 0 auto;
                            }}
                            .message {{
                                color: #555;
                                font-size: 16px;
                                line-height: 1.6;
                                margin-bottom: 25px;
                                text-align: center;
                            }}
                            .footer {{
                                margin-top: 25px;
                                color: #7f8c8d;
                                font-size: 14px;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='email-container'>
                            <div class='content'>
                                <div class='header'>
                                    <h1>Your Reservation Confirmation</h1>
                                </div>
                    
                                <div class='message'>
                                    {body}
                                </div>
                    
                                <div class='image-container'>
                                    <img src='cid:{image.ContentId}' alt='Reservation Image' class='reservation-image' />
                                </div>
                    
                                <div class='footer'>
                                    Thank you for using Sufra!
                                </div>
                            </div>
                        </div>
                    </body>
                </html>";

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

            Console.WriteLine("Email sent successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
        }
    }
    public async Task SendRejectionEmailAsync(string toEmail, string subject)
    {
        var emailMessage = new MimeMessage();

        // Set sender's email address
        emailMessage.From.Add(new MailboxAddress("Sufra", "sufraa.app@gmail.com"));

        // Set recipient's email address
        emailMessage.To.Add(new MailboxAddress("", toEmail));

        // Set subject
        emailMessage.Subject = subject;

        var bodyBuilder = new BodyBuilder();

        // Set both text and HTML bodies
        bodyBuilder.TextBody = "Unfortunately your reservation has been rejected. Please try again or contact our support team.";
        bodyBuilder.HtmlBody = $@"
    <html>
        <head>
            <style>
                body {{
                    font-family: 'Arial', sans-serif;
                    margin: 0;
                    padding: 0;
                    background-color: #f8f9fa;
                    color: #333;
                }}
                .email-container {{
                    max-width: 600px;
                    margin: 0 auto;
                    padding: 20px;
                }}
                .content {{
                    background-color: #ffffff;
                    padding: 30px;
                    border-radius: 12px;
                    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
                    text-align: center;
                }}
                .header {{
                    margin-bottom: 25px;
                }}
                h1 {{
                    color: #d9534f; /* Red color for rejection */
                    font-size: 24px;
                    margin-bottom: 20px;
                }}
                .message {{
                    color: #555;
                    font-size: 16px;
                    line-height: 1.6;
                    margin-bottom: 25px;
                    text-align: center;
                }}
                .action {{
                    margin: 25px 0;
                }}
                .support-info {{
                    background-color: #f8f9fa;
                    padding: 15px;
                    border-radius: 8px;
                    margin-top: 20px;
                }}
                .footer {{
                    margin-top: 25px;
                    color: #7f8c8d;
                    font-size: 14px;
                }}
            </style>
        </head>
        <body>
            <div class='email-container'>
                <div class='content'>
                    <div class='header'>
                        <h1>Reservation Not Confirmed</h1>
                    </div>
                    
                    <div class='message'>
                        <p>We regret to inform you that your reservation request could not be processed.</p>
                        <p>This might be due to unavailable slots or other restrictions.</p>
                    </div>
                    
                    <div class='action'>
                        <p>Please try making a new reservation or contact our support team for assistance.</p>
                    </div>
                    
                    <div class='support-info'>
                        <p><strong>Support Contact:</strong></p>
                        <p>Email: support@sufra.com</p>
                        <p>Phone: +1 (123) 456-7890</p>
                    </div>
                    
                    <div class='footer'>
                        <p>We apologize for any inconvenience and thank you for your understanding.</p>
                        <p>The Sufra Team</p>
                    </div>
                </div>
            </div>
        </body>
    </html>";

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
