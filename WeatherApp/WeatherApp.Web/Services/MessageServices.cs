using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace WeatherApp.Web.Services
{
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var sender = new MailboxAddress("Администрация сайта", "alexandr.limanscky@gmail.com");
            var receiver = new MailboxAddress("", email);
            var emailMessage = new MimeMessage()
            {
                Subject = subject,
                Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = message
                }
            };
            emailMessage.From.Add(sender);
            emailMessage.To.Add(receiver);

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync(sender.Address, "Reira147Zange369");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
