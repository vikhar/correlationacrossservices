using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace WebApi
{
    internal static class ExceptionHandlerExtensions
    {
        public static bool Handle(this ExceptionHandlerContext context, Exception exception)
        {
            var innerExceptionType = exception.InnerException?.GetType();
            var exceptionType = innerExceptionType ?? exception.GetType();
           
            return true;
        }
    }
}
