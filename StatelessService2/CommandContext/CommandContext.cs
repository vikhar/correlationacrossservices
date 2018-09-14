using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatelessService2.CommandContext.Handlers;
using TracingUtil;

namespace StatelessService2.CommandContext
{
    public class CommandContext
    {
        private readonly Dictionary<string, ICommandContext> _contextListeners;

        public CommandContext()
        {
            //Add handlers here
            _contextListeners = new Dictionary<string, ICommandContext>()
            {
                {
                    "Handler1", new Handler1()
                 }

            };
        }

        public TK DoAction<T, TK>(string command, T input)
        {
            return _contextListeners[command].DoAction<T, TK>(input);
        }
    }
}
