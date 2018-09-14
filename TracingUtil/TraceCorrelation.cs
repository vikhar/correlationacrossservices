using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracingUtil
{

    /// <summary>
    /// TraceCorrelation For Code Instrumentation 
    /// </summary>
    public static class TraceCorrelation
    {
        /// <summary>
        /// RetrieveCorrelationInfo
        /// </summary>
        /// <returns></returns>
        public static string RetrieveCorrelationInfo()
        {
            var correlationId = ServiceTracingContext.GetRequestCorrelationId();
            UpdateCallContext(correlationId);
            return correlationId;
        }

        /// <summary>
        /// UpdateCallContext
        /// </summary>
        /// <param name="correlationId"></param>
        private static void UpdateCallContext(string correlationId)
        {
            try
            {
                CorrelationDetails.UpdateCurrentActivityId(correlationId);
            }
            catch (Exception ex)
            {
                ServiceEventSource.Current.Message(ex?.InnerException?.ToString(), $"CorrelationId:{ correlationId }; {ex.StackTrace}");
            }
        }

        /// <summary>
        /// GetCurrentActivityId
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentActivityId()
        {
            string currentActivityId = string.Empty;

            if (!CorrelationDetails.TryGetCurrentActivityId(out currentActivityId))
            {
                ServiceEventSource.Current.Message("GetCurrentActivityId", "Current USPCorrelationId not found!");
            }
            UpdateCallContext(currentActivityId);

            return Task.FromResult(currentActivityId).Result;
        }

    }
}
