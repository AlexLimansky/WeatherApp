using System.Threading.Tasks;

namespace WeatherApp.Web.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}