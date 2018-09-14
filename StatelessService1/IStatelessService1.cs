using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace StatelessService1
{
    public interface IStatelessService1 : IService
    {
        Task<string> Service1Method1(string param1);

    }
}
