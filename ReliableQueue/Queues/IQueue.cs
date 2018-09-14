// ************************************************************************
// *****      COPYRIGHT 2014 - 2016 HONEYWELL INTERNATIONAL SARL      *****
// ************************************************************************

using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data;

namespace ReliableQueue.Queues
{
    public interface IQueue<T>
    {
        Task Enqueue(ITransaction tx, T item);

        Task<ConditionalValue<T>> Dequeue(ITransaction tx, CancellationToken cancellationToken);
    }
}
