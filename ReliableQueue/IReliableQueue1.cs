using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace ReliableQueue
{
    public interface IReliableQueue1 : IService
    {
        Task QueueMethod1(string param1);

        Task StartScheduledMaterializedViewProcessing(string orgData);
    }
}
