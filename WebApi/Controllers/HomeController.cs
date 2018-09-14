using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ReliableQueue;
using StatelessService1;
using TracingUtil;

namespace WebApi.Controllers
{
    public class HomeController: ApiController
    {
        private readonly IServiceProxyFactory<IStatelessService1> _service1Proxy;
        private readonly IServiceProxyFactory<IReliableQueue1> _queueProxy;


        public HomeController(IServiceProxyFactory<IStatelessService1> service1Proxy,
            IServiceProxyFactory<IReliableQueue1> queueProxy)
        {
            _service1Proxy = service1Proxy;
            _queueProxy = queueProxy;
        }

        [HttpGet]
        [ResponseType(typeof(string))]
        [Route("api/method1")]
        public async Task<IHttpActionResult> Method1(string param1)
        {
            var correlationId = TraceCorrelation.RetrieveCorrelationInfo();
            var absoluteUrl = Request?.RequestUri?.AbsoluteUri;
            //var requestTypeToLog = absoluteUrl + "; Param1: " + param1 +"; " + correlationId;

            WebApiEventSource.Current.ServiceRequestStart(absoluteUrl, correlationId, "One");
            try
            {
                var serviceProxy = _service1Proxy.CreateSingletonServiceProxy();
                var organization = await serviceProxy.Service1Method1(param1);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           

            WebApiEventSource.Current.ServiceRequestStop(absoluteUrl, correlationId);
            return Ok("Return from API method 1");
        }


        [HttpGet]
        [ResponseType(typeof(string))]
        [Route("api/queue")]
        public async Task<IHttpActionResult> MethodQueue(string param1)
        {
            var correlationId = TraceCorrelation.RetrieveCorrelationInfo();
            var absoluteUrl = Request?.RequestUri?.AbsoluteUri;
            //var requestTypeToLog = absoluteUrl + "; Param1: " + param1 +"; " + correlationId;

            WebApiEventSource.Current.ServiceRequestStart(absoluteUrl, correlationId, "One");

            var serviceProxy = _queueProxy.CreateSingletonServiceProxy();
            await serviceProxy.StartScheduledMaterializedViewProcessing(param1);

            WebApiEventSource.Current.ServiceRequestStop(absoluteUrl, correlationId);
            return Ok("Return from API method 1");
        }
    }
}
