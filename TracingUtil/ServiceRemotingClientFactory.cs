using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Client;


namespace TracingUtil
{
    public class ServiceRemotingClientFactory : IServiceRemotingClientFactory
    {
        private IServiceRemotingClientFactory _serviceRemotingClientFactory;

        public ServiceRemotingClientFactory(
            IServiceRemotingCallbackClient callbackClient,
            IServicePartitionResolver resolver = null,
            IEnumerable<IExceptionHandler> exceptionHandlers = null)
        {
            _serviceRemotingClientFactory = new FabricTransportServiceRemotingClientFactory(
                new FabricTransportRemotingSettings(),
                callbackClient,
                resolver,
                exceptionHandlers);

            _serviceRemotingClientFactory.ClientConnected += ClientConnected;
            _serviceRemotingClientFactory.ClientDisconnected += ClientDisconnected;
        }

        public event EventHandler<CommunicationClientEventArgs<IServiceRemotingClient>> ClientConnected;

        public event EventHandler<CommunicationClientEventArgs<IServiceRemotingClient>> ClientDisconnected;

        public async Task<IServiceRemotingClient> GetClientAsync(
            ResolvedServicePartition previousRsp, TargetReplicaSelector targetReplicaSelector, string listenerName, OperationRetrySettings retrySettings,
            CancellationToken cancellationToken)
        {
            IServiceRemotingClient remotingClient = await _serviceRemotingClientFactory.GetClientAsync(
                previousRsp,
                targetReplicaSelector,
                listenerName,
                retrySettings,
                cancellationToken);

            return new ServiceRemotingClient(remotingClient);
        }

        public async Task<IServiceRemotingClient> GetClientAsync(
            Uri serviceUri, ServicePartitionKey partitionKey, TargetReplicaSelector targetReplicaSelector, string listenerName,
            OperationRetrySettings retrySettings, CancellationToken cancellationToken)
        {
            IServiceRemotingClient remotingClient = await _serviceRemotingClientFactory.GetClientAsync(
                serviceUri,
                partitionKey,
                targetReplicaSelector,
                listenerName,
                retrySettings,
                cancellationToken);

            return new ServiceRemotingClient(remotingClient);
        }

        public async Task<OperationRetryControl> ReportOperationExceptionAsync(
            IServiceRemotingClient client, ExceptionInformation exceptionInformation, OperationRetrySettings retrySettings, CancellationToken cancellationToken)
        {
            return await _serviceRemotingClientFactory.ReportOperationExceptionAsync(
                ((ServiceRemotingClient)client)._serviceRemotingClient,
                exceptionInformation,
                retrySettings,
                cancellationToken);
        }
    }
}
