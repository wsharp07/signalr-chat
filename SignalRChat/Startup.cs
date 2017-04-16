using System;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using SignalRChat.Lib;
using SimpleInjector;

[assembly: OwinStartup(typeof(SignalRChat.Startup))]
namespace SignalRChat
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = new Container();
            var resolver = new SimpleInjectorDependencyResolver(container);
            var config = new HubConfiguration();

            container.Register<IConnectionMapping<string>, ConnectionMapping<string>>(Lifestyle.Singleton);

            config.Resolver = resolver;

            // Any connection or hub wire up and configuration should go here
            app.MapSignalR(config);

            
        }
    }
}