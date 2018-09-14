// ************************************************************************
// *****      COPYRIGHT 2014 - 2016 HONEYWELL INTERNATIONAL SARL      *****
// ************************************************************************

using Microsoft.ServiceFabric.Data;

namespace ReliableQueue.Queues
{
    public class QueueFactory : IQueueFactory
    {
        private readonly IReliableStateManager _stateManager;

        public QueueFactory(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public IQueue<T> CreateQueue<T>(TriggerType type)
        {
            if (type == TriggerType.Scheduled)
            {
                return new ScheduledQueue<T>(_stateManager);
            }

            return new ScheduledQueue<T>(_stateManager);
        }
    }
}
