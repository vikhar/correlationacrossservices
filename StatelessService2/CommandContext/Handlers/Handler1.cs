using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TracingUtil;

namespace StatelessService2.CommandContext.Handlers
{
    public class Handler1 : ICommandContext
    {
        public TK DoAction<T, TK>(T input)
        {
            var result = default(TK);
            var correlationId = TraceCorrelation.GetCurrentActivityId();
            ServiceEventSource.Current.ServiceRequestStart("Handler1 - Start; " + correlationId, correlationId);
            Thread.Sleep(1000);
            ServiceEventSource.Current.ServiceRequestStop("Handler1 - Stop; " + correlationId, correlationId);

            return result;
        }
    }
}
