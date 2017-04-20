using System;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using SimpleInjector;

namespace SignalRChat.Server
{
    public class SimpleInjectorDependencyResolver : DefaultDependencyResolver
    {
        private readonly Container _container;

        public SimpleInjectorDependencyResolver(Container container)
        {
            _container = container;
        }

        public override object GetService(Type serviceType)
        {
            try
            {
                return _container.GetInstance(serviceType);
            }
            catch (ActivationException)
            {
                return base.GetService(serviceType);
            }
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _container.GetAllInstances(serviceType);
            }
            catch (ActivationException)
            {
                return base.GetServices(serviceType);
            }
        }
    }
}