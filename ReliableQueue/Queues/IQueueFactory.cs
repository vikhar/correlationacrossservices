// ************************************************************************
// *****      COPYRIGHT 2014 - 2016 HONEYWELL INTERNATIONAL SARL      *****
// ************************************************************************

namespace ReliableQueue.Queues
{
    public interface IQueueFactory
    {
       IQueue<T> CreateQueue<T>(TriggerType type);
    }
}
