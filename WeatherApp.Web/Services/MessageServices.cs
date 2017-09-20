using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using WeatherApp.Web.Options;

namespace WeatherApp.Web.Services
{
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private EmailOptions options;

        public AuthMessageSender(IOptions<EmailOptions> options)
        {
            this.options = options.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var sender = new MailboxAddress(this.options.SenderName, this.options.SenderAdress);
            var receiver = new MailboxAddress(string.Empty, email);
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
                await client.ConnectAsync(this.options.SenderHost, this.options.SenderPort, this.options.UseSsl);
                await client.AuthenticateAsync(this.options.SenderAdress, this.options.SenderPass);
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