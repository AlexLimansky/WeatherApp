using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(GuessTheNumberWebAPI.Startup))]

namespace GuessTheNumberWebAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
