using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace TracingUtil
{
    public class CorrelationDetails
    {
        private const string _activityIdKeyName = "uspcorrelationId";

        public static void SetActivityIdHeader(ServiceRemotingMessageHeaders headers)
        {
            string activityId = GetOrCreateActivityId();
            headers.AddHeader(_activityIdKeyName, Encoding.UTF8.GetBytes(activityId));
        }

        public static void UpdateCurrentActivityId(ServiceRemotingMessageHeaders headers)
        {
            byte[] headerValue;
            if (!headers.TryGetHeaderValue(_activityIdKeyName, out headerValue))
            {
                return;
            }

            UpdateCurrentActivityId(Encoding.UTF8.GetString(headerValue));
        }

        public static void UpdateCurrentActivityId(string activityId)
        {
            CallContext.LogicalSetData(_activityIdKeyName, activityId);
        }

        public static bool TryGetCurrentActivityId(out string activityId)
        {
            activityId = (string)CallContext.LogicalGetData(_activityIdKeyName);
            return (activityId != null);
        }

        public static string GetOrCreateActivityId()
        {
            string activityId;
            if (!TryGetCurrentActivityId(out activityId))
            {
                activityId = Guid.NewGuid().ToString();
                UpdateCurrentActivityId(activityId);
            }

            return activityId;
        }
    }
}
