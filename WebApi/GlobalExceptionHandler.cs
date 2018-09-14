using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using TracingUtil;
using WebApi.Controllers;

namespace WebApi
{
    using System;
    using System.Web.Http.ExceptionHandling;

    internal sealed class GlobalExceptionHandler : ExceptionHandler
    {
        /// <summary>
        /// Handles the exception
        /// </summary>
        /// <param name="context"></param>
        public override void Handle(ExceptionHandlerContext context)
        {
            var exception = context.Exception;
            if (context.Handle(exception))
            {
                return;
            }
            var correlationId = TraceCorrelation.RetrieveCorrelationInfo();
           
            if (exception.InnerException != null)
            {
                var errorMessage = exception.InnerException.Message;
                var stackTrace = exception.InnerException.StackTrace ?? "No stack trace available";
                
            }
        }
    }
}
