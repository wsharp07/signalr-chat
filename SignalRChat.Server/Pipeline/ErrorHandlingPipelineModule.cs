using Microsoft.AspNet.SignalR.Hubs;
using SignalRChat.Server.Services;

namespace SignalRChat.Server.Pipeline
{
    public class ErrorHandlingPipelineModule : HubPipelineModule
    {
        private readonly ILogger _logger;
        public ErrorHandlingPipelineModule(ILogger logger)
        {
            _logger = logger;
        }
        protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext)
        {
            _logger.Error(exceptionContext.Error.Message);

            if (exceptionContext.Error.InnerException != null)
            {
                _logger.Error(exceptionContext.Error.InnerException.Message);
            }

            base.OnIncomingError(exceptionContext, invokerContext);
        }
    }
}
