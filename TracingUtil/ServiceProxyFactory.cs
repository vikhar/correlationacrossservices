using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace TracingUtil
{
    public class ServiceProxyFactory<T> : IServiceProxyFactory<T> where T : IService
    {
        private readonly Uri _serviceUri;

        /// <summary>
        /// Creates an instance of ServiceProxyFactory.
        /// </summary>
        /// <param name="serviceUri">Service fabric unique uri corresponding to the stateful service.</param>
        public ServiceProxyFactory(Uri serviceUri)
        {
            _serviceUri = serviceUri;
        }

        /// <summary>
        /// Creates a proxy for the specified service.
        /// </summary>
        /// <param name="partitionKey">Stateful service partition key.</param>
        /// <returns></returns>
        public T CreateServiceProxy(long partitionKey)
        {
            return ServiceProxy.Create<T>(_serviceUri, new ServicePartitionKey(partitionKey));
        }

        /// <summary>
        /// Creates a proxy for the specified service.
        /// </summary>
        /// <param name="partitionKey">Stateful service partition key.</param>
        /// <returns></returns>
        public T CreateServiceProxy(string partitionKey)
        {
            return ServiceProxy.Create<T>(_serviceUri, new ServicePartitionKey(partitionKey));
        }

        public T CreateSingletonServiceProxy()
        {
            var proxyFactory = new ServiceProxyFactory(callbackClient => new ServiceRemotingClientFactory(callbackClient));
            return proxyFactory.CreateServiceProxy<T>(_serviceUri);
        }

    }
}
