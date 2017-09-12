using System.Threading.Tasks;

namespace WeatherApp.Web.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}