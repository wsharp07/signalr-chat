using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;
using SimpleInjector;

namespace SignalRChat.Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            var container = new Container();
            var resolver = new SimpleInjectorDependencyResolver(container);
            var config = new HubConfiguration();

            container.Register<IConnectionMapping<string>, ConnectionMapping<string>>(Lifestyle.Singleton);
            container.Register<ILogger, ConsoleLogger>();

            config.Resolver = resolver;

            // Any connection or hub wire up and configuration should go here
            app.MapSignalR(config);
        }
    }
}
