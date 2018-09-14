using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using StatelessService2;
using TracingUtil;

namespace StatelessService1
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class StatelessService1 : StatelessService, IStatelessService1
    {
        private readonly IServiceProxyFactory<IStatelessService2> _statelessService2ProxyFactory;

        public StatelessService1(StatelessServiceContext context,
            IServiceProxyFactory<IStatelessService2> statelessService2ProxyFactory)
            : base(context)
        {
            _statelessService2ProxyFactory = statelessService2ProxyFactory;
        }


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

            //for (int i = 0; i < 3; i++)
            //{
            //    await Service1Method1();
            //}

            //while (true)
            //{
            //    cancellationToken.ThrowIfCancellationRequested();

            //    ServiceEventSource.Current.ServiceMessage(this.Context, "Working-{0}", ++iterations);

            //    await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            //}
        }

        public async Task<string> Service1Method1(string param1)
        {
            try
            {
                var x = 1;
                var y = 0;

                if(y==0)
                return (x / y).ToString();

                var correlationId = TraceCorrelation.GetCurrentActivityId();
                ServiceEventSource.Current.ServiceRequestStart("Service1Method1", correlationId);
                ServiceEventSource.Current.ServiceRequestStart1(correlationId, "Service1Method1");
                var proxy = _statelessService2ProxyFactory.CreateSingletonServiceProxy();
                var result = await proxy.Service2Method1(param1);
                ServiceEventSource.Current.ServiceRequestStop("Service1Method1", correlationId);
                return result;


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
    }
}
