// ************************************************************************
// *****      COPYRIGHT 2014 - 2016 HONEYWELL INTERNATIONAL SARL      *****
// ************************************************************************

using Microsoft.ServiceFabric.Data;

namespace ReliableQueue.Queues
{
    public interface IQueueHandler
    {
        IQueueFactory CreateFactory(IReliableStateManager stateManager);
    }
}
