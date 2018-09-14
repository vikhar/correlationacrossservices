using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatelessService2.CommandContext
{
    public interface ICommandContext
    {
        TK DoAction<T, TK>(T input);
        
    }
}
