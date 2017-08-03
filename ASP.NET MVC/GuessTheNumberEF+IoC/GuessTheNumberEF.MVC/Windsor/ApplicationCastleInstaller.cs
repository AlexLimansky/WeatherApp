using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Data.Entity;
using GuessTheNumberEF.Data.Utils;
using GuessTheNumberEF.Logic;
using GuessTheNumberEF.Data.Contexts;

namespace GuessTheNumberEF.MVC
{
    public class ApplicationCastleInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For(typeof(IRepository<>)).ImplementedBy(typeof(GenericRepository<>)));
            container.Register(Component.For<IGameManager>().ImplementedBy<GameManagerMVC>());
            container.Register(Component.For<DbContext>().ImplementedBy<DataContext>());
            var controllers = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.BaseType == typeof(Controller)).ToList();
            foreach (var controller in controllers)
            {
                container.Register(Component.For(controller).LifestylePerWebRequest());
            }
        }
    }
}