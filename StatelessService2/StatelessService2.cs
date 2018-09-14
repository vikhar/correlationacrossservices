using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Description;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Runtime;
using TracingUtil;

namespace StatelessService2
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class StatelessService2 : StatelessService, IStatelessService2
    {
        private CommandContext.CommandContext _commandContext;
        public StatelessService2(StatelessServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[]
        {
                new ServiceInstanceListener(
                    context =>
                        new Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Runtime.FabricTransportServiceRemotingListener(
                                this.Context,
                                new ServiceRemotingMessageHandler(context, this)))
            };
        }


        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            long iterations = 0;

            _commandContext = new CommandContext.CommandContext();

            //while (true)
            //{
            //    cancellationToken.ThrowIfCancellationRequested();

            //    ServiceEventSource.Current.ServiceMessage(this.Context, "Working-{0}", ++iterations);

            //    await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            //}
        }

        public async Task<string> Service2Method1(string param1)
        {
            var correlationId = TraceCorrelation.GetCurrentActivityId();
            ServiceEventSource.Current.ServiceRequestStart("Service2 Method1 - Start; PARAM1: " + param1 + "; " + correlationId, correlationId);
            await Service2PrivateMethod1();
            ServiceEventSource.Current.ServiceRequestStop("Service2 Method1 - End; PARAM1: " + param1 + "; " + correlationId, correlationId);
            return "Result from Service 2, Method 1";
        }

        private async Task<string> Service2PrivateMethod1()
        {
            var correlationId = TraceCorrelation.GetCurrentActivityId();

            ServiceEventSource.Current.ServiceRequestStart("Service2 Private Method1 - Start" + correlationId, correlationId);
            Thread.Sleep(2000);
            var result = Task.Run(
                () => _commandContext.DoAction<int, bool>("Handler1", 5));
            ServiceEventSource.Current.ServiceRequestStop("Service2 Private Method1 - End;" + correlationId, correlationId);

            return "Result from Service 2, Method 1";
        }
    }
}
