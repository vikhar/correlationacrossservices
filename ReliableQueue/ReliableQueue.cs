using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using ReliableQueue.Queues;
using TracingUtil;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;


namespace ReliableQueue
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class ReliableQueue : StatefulService, IReliableQueue1
    {
        private readonly IQueueHandler _queueHandler;

        private IQueueFactory _queueFactory;

        public ReliableQueue(StatefulServiceContext context, IQueueHandler queueHandler)
            : base(context)
        {
            _queueHandler = queueHandler;
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
           {
                new ServiceReplicaListener(
                    context =>
                        new Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Runtime.FabricTransportServiceRemotingListener(
                                this.Context,
                                new ServiceRemotingMessageHandler(context, this)))
            };
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            _queueFactory = _queueHandler.CreateFactory(StateManager);

           
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                using (var tx = StateManager.CreateTransaction())
                {
                    var scheduledQueue = _queueFactory.CreateQueue<string>(TriggerType.Scheduled);
                    var result = await scheduledQueue.Dequeue(tx, cancellationToken);

                    if (result.HasValue)
                    {
                        Console.WriteLine(result.Value);
                    }

                    await tx.CommitAsync();
                }

            }
        }

        public Task QueueMethod1(string param1)
        {
            throw new NotImplementedException();
        }

        public async Task StartScheduledMaterializedViewProcessing(string orgData)
        {
            // Take the Org and Site ID from this and write into a queue.
            try
            {
                //await WriteToScheduledQueue(orgData);
                var scheduledQueue = _queueFactory.CreateQueue<string>(TriggerType.Scheduled);

                using (var tx = StateManager.CreateTransaction())
                {
                    await scheduledQueue.Enqueue(tx, orgData);

                    await tx.CommitAsync();
                }
            }
            catch (Exception e)
            {
                //TODO: Send excpetion up to calling API. decide on error codes.
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
