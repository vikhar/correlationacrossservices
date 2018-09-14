using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.Client;


namespace TracingUtil
{
    public class ServiceRemotingClient : IServiceRemotingClient
    {
        public IServiceRemotingClient _serviceRemotingClient;

        public ServiceRemotingClient(IServiceRemotingClient remotingClient)
        {
            _serviceRemotingClient = remotingClient;
        }

        public ResolvedServiceEndpoint Endpoint
        {
            get { return _serviceRemotingClient.Endpoint; }

            set { _serviceRemotingClient.Endpoint = value; }
        }

        public string ListenerName
        {
            get { return _serviceRemotingClient.ListenerName; }

            set { _serviceRemotingClient.ListenerName = value; }
        }

        public ResolvedServicePartition ResolvedServicePartition
        {
            get { return _serviceRemotingClient.ResolvedServicePartition; }

            set { _serviceRemotingClient.ResolvedServicePartition = value; }
        }

        public async Task<byte[]> RequestResponseAsync(ServiceRemotingMessageHeaders messageHeaders, byte[] requestBody)
        {
            CorrelationDetails.SetActivityIdHeader(messageHeaders);
            return await _serviceRemotingClient.RequestResponseAsync(messageHeaders, requestBody);
        }

        public void SendOneWay(ServiceRemotingMessageHeaders messageHeaders, byte[] requestBody)
        {
            CorrelationDetails.SetActivityIdHeader(messageHeaders);
            _serviceRemotingClient.SendOneWay(messageHeaders, requestBody);
        }
    }
}
