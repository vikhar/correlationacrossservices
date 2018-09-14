using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Description;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace WebApi
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class WebApi : StatelessService
    {
        private readonly StatelessServiceContext _context;

        public WebApi(StatelessServiceContext context)
            : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            var endpoints =
                Context.CodePackageActivationContext.GetEndpoints()
                    .Where(
                        endpoint =>
                            endpoint.Protocol == EndpointProtocol.Http || endpoint.Protocol == EndpointProtocol.Https)
                    .Select(endpoint => endpoint.Name);

            return
                endpoints.Select(
                    endpoint =>
                        new ServiceInstanceListener(
                            serviceContext =>
                                new OwinCommunicationListener(
                                    appbuilder => new Startup().ConfigureApp(appbuilder, _context),
                                    serviceContext,
                                    WebApiEventSource.Current,
                                    endpoint),
                            endpoint));
        }


    }
}
