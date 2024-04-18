using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using RestAPI.Models.DTOs;
using MailKit.Net.Smtp;
using RestAPI.Services.EmailService;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace RestAPI.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public void SendEmail(EmailDto request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(request.from));
            email.To.Add(MailboxAddress.Parse("Usamabalti3377@gmail.com"));

            email.Subject = request.subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.body };
            using var smtp = new SmtpClient();

            smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}




