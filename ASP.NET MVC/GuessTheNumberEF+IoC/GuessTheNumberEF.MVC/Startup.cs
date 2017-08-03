using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GuessTheNumberEF.MVC.Startup))]
namespace GuessTheNumberEF.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
