using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;

namespace TracingUtil
{
    public class ServiceTracingMiddleware :OwinMiddleware
    {
        ServiceContext _serviceContext;

        public ServiceTracingMiddleware(OwinMiddleware next, ServiceContext context) : base(next)
        {
            _serviceContext = context;
        }

        public async override Task Invoke(IOwinContext context)
        {
            var serviceDetails = string.Format("{0}/{1}/{2}",
                _serviceContext != null ? _serviceContext.ServiceName.ToString() : "ServiceContext is null",
                _serviceContext != null ? _serviceContext.PartitionId : Guid.Empty,
                _serviceContext != null ? (_serviceContext as StatelessServiceContext).InstanceId : 0);

            // For end-to-end tracing we can also generate the CorrelationId in the web app, pass it in header and extract it here.
            //Putting check to generate New ID only when its not coming from UI

            var correlationId = string.Empty;

            if (HttpContext.Current != null)
            {
                correlationId = HttpContext.Current.Request.Headers["UspCorrelationId"];
            }

            if (string.IsNullOrWhiteSpace(correlationId))
            {
                ServiceTracingContext.CreateRequestCorrelationId();
                ServiceEventSource.Current.Message("New USPCorrelationId is created " + ServiceTracingContext.GetRequestCorrelationId());
            }
            else
            {
                ServiceTracingContext.SetRequestCorrelationId(correlationId);
            }

            ServiceTracingContext.SetRequestServiceDetails(serviceDetails);

            await Next.Invoke(context);
        }
    }
}
