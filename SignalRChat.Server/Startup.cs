using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;
using SignalRChat.Server.Pipeline;
using SignalRChat.Server.Services;
using SimpleInjector;

namespace SignalRChat.Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = new Container();
            var resolver = new SimpleInjectorDependencyResolver(container);
            var config = new HubConfiguration {EnableDetailedErrors = true};

            // Register
            container.Register<IConnectionMapping<string>, ConnectionMapping<string>>(Lifestyle.Singleton);
            container.Register<ILogger, ConsoleLogger>();

            // Assign resolvers
            config.Resolver = resolver;
            GlobalHost.DependencyResolver = resolver;

            // Add pipelines
            var errorPipeline = container.GetInstance<ErrorHandlingPipelineModule>();
            GlobalHost.HubPipeline.AddModule(errorPipeline);

            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR(config);
        }
    }
}
