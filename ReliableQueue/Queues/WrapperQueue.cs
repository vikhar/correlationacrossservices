using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace ReliableQueue.Queues
{
    public abstract class WrapperQueue<T> : IQueue<T>
    {
        private readonly IReliableStateManager _stateManager;
        private readonly SemaphoreSlim _signal;

        private readonly string _queueName;

        protected WrapperQueue(IReliableStateManager stateManager, string queueName)
        {
            _stateManager = stateManager;
            _queueName = queueName;
            _signal = new SemaphoreSlim(1);
        }

        public virtual async Task<ConditionalValue<T>> Dequeue(ITransaction scope, CancellationToken cancellationToken)
        {
            var specDataQueue = await _stateManager.GetOrAddAsync<IReliableQueue<T>>(_queueName);

            try
            {
                await _signal.WaitAsync(cancellationToken);


                var result = await specDataQueue.TryDequeueAsync(scope);

                var countDiff = await GetCountDiff(scope);

                if (countDiff > 0)
                    _signal.Release(countDiff);


                return result;
            }
            catch (FabricNotReadableException ex)
            {
                throw;
            }

        }

        public virtual async Task Enqueue(ITransaction scope, T item)
        {


            var specDataQueue = await _stateManager.GetOrAddAsync<IReliableQueue<T>>(_queueName);

            long queueCount = 0;

            try
            {
                await specDataQueue.EnqueueAsync(scope, item);
                _signal.Release();
                //queueCount = await specDataQueue.GetCountAsync(scope);
            }
            catch (FabricNotReadableException ex)
            {
            }
        }

        private async Task<int> GetCountDiff(ITransaction tx)
        {
            var specDataQueue = await _stateManager.GetOrAddAsync<IReliableQueue<T>>(_queueName);

            return (int)await specDataQueue.GetCountAsync(tx).ConfigureAwait(false) - _signal.CurrentCount;
        }
    }
}
