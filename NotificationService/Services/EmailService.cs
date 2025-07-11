using MimeKit;
using System.Net.Mail;

namespace NotificationService.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpSection = _configuration.GetSection("SmtpSettings");

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(
                smtpSection["FromName"],
                smtpSection["FromEmail"]
                ));

            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using var client = new MailKit.Net.Smtp.SmtpClient(); 

            await client.ConnectAsync(
                smtpSection["Host"],
                int.Parse(smtpSection["Port"]),
                false
            );

            await client.AuthenticateAsync(
           smtpSection["UserName"],
           smtpSection["Password"]
            );


            await client.SendAsync(message);
            await client.DisconnectAsync(true); 
        }
    }
}
