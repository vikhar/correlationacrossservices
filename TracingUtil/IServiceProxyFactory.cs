using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace TracingUtil
{
    public interface IServiceProxyFactory<T> where T : IService
    {
        /// <summary>
        /// Creates a proxy for the specified service.
        /// </summary>
        /// <param name="partitionKey">Stateful service partition key.</param>
        /// <returns></returns>
        T CreateServiceProxy(long partitionKey);

        /// <summary>
        /// Creates a proxy for the specified service.
        /// </summary>
        /// <param name="partitionKey">Stateful service partition key.</param>
        /// <returns></returns>
        T CreateServiceProxy(string partitionKey);

        /// <summary>
        /// Creates a proxy for the specified service.
        /// </summary>
        /// <returns></returns>
        T CreateSingletonServiceProxy();
    }
}
