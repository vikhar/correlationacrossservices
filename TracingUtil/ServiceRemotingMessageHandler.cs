using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace TracingUtil
{
    public class ServiceRemotingMessageHandler : IServiceRemotingMessageHandler
    {
        private ServiceRemotingDispatcher _serviceRemotingMessageHandler;

        public ServiceRemotingMessageHandler(ServiceContext context, IService service)
        {
            _serviceRemotingMessageHandler = new ServiceRemotingDispatcher(context, service);
        }

        public void HandleOneWay(IServiceRemotingRequestContext requestContext, ServiceRemotingMessageHeaders messageHeaders, byte[] requestBody)
        {
            CorrelationDetails.UpdateCurrentActivityId(messageHeaders);
            _serviceRemotingMessageHandler.HandleOneWay(requestContext, messageHeaders, requestBody);
        }

        public async Task<byte[]> RequestResponseAsync(
            IServiceRemotingRequestContext requestContext, ServiceRemotingMessageHeaders messageHeaders, byte[] requestBody)
        {
            try
            {
                CorrelationDetails.UpdateCurrentActivityId(messageHeaders);
                return await _serviceRemotingMessageHandler.RequestResponseAsync(requestContext, messageHeaders, requestBody);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
                
        }
    }
}
