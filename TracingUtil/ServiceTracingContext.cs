using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace TracingUtil
{
    public class ServiceTracingContext
    {
        const string _correlationKey = "CorrelationId";
        const string _serviceDetailsKey = "ServiceDetails";

        public static void CreateRequestCorrelationId()
        {
            CallContext.LogicalSetData(_correlationKey, GenerateId());
        }

        public static string GetRequestCorrelationId()
        {
            return CallContext.LogicalGetData(_correlationKey) as string;
        }

        public static void SetRequestCorrelationId(string value)
        {
            CallContext.LogicalSetData(_correlationKey, value);
        }


        public static string GetRequestServiceDetails()
        {
            return CallContext.LogicalGetData(_serviceDetailsKey) as string;
        }

        public static void SetRequestServiceDetails(string value)
        {
            CallContext.LogicalSetData(_serviceDetailsKey, value);
        }

        private static string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
