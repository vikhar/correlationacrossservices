// ************************************************************************
// *****      COPYRIGHT 2014 - 2016 HONEYWELL INTERNATIONAL SARL      *****
// ************************************************************************

using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data;

namespace ReliableQueue.Queues
{
    public class ScheduledQueue<T> : WrapperQueue<T>
    {
        private const string ScheduledTriggerQueue = "SpecScheduledQueue";


        public ScheduledQueue(IReliableStateManager stateManager) : base(stateManager, ScheduledTriggerQueue)
        {
        }

        public override async Task Enqueue(ITransaction scope, T orgData)
        {
            await base.Enqueue(scope, orgData);
        }

        public override async Task<ConditionalValue<T>> Dequeue(ITransaction scope, CancellationToken cancellationToken)
        {
           return await base.Dequeue(scope, cancellationToken);
        }
    }
}
