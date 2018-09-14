// ************************************************************************
// *****      COPYRIGHT 2014 - 2016 HONEYWELL INTERNATIONAL SARL      *****
// ************************************************************************

using Microsoft.ServiceFabric.Data;

namespace ReliableQueue.Queues
{
    public class QueueHandler : IQueueHandler
    {
        private IReliableStateManager _stateManager;

        public IQueueFactory CreateFactory(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
            return new QueueFactory(_stateManager);
        }
    }
}
