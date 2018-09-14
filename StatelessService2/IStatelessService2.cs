using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace StatelessService2
{
    public interface IStatelessService2 : IService
    {
        Task<string> Service2Method1(string param1);
    }
}
