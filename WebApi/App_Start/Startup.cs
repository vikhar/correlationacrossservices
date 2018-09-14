using System;
using System.Fabric;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Autofac;
using Autofac.Integration.WebApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Owin;
using ReliableQueue;
using StatelessService1;
using TracingUtil;

namespace WebApi
{
    /// <summary>
    /// Startup class for WebAPi
    /// </summary>
    internal partial class Startup
    {
        public void ConfigureApp(IAppBuilder appBuilder, StatelessServiceContext context)
        {
            var config = new HttpConfiguration();


            ////Filter for badRequest Logging
            //config.Filters.Add(new GlobalErrorResponseFilter());

            //Global Exception Handler
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());

            // Configure web api routes
            config.MapHttpAttributeRoutes();

            //appBuilder.Use<LicenseMangerComponent>(context);

            //Tracing-Code Instrumendation
            appBuilder.Use<ServiceTracingMiddleware>(context);
            appBuilder.UseWebApi(config);



            // Configure DI
            try
            {
                ConfigureDependencyInjection(config, context);

                var jsonSetting = new JsonSerializerSettings()
                { // Removes Trailing whitespace in Model Objects
                    Formatting = Formatting.None
                };
                jsonSetting.Converters.Add(new StringEnumConverter());
                config.Formatters.JsonFormatter.SerializerSettings = jsonSetting;

                config.EnsureInitialized();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        public void ConfigureDependencyInjection(HttpConfiguration config, StatelessServiceContext context)
        {
            var builder = new ContainerBuilder();

            ConfigureServiceProxies(builder, context);
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());


            config.DependencyResolver = new AutofacWebApiDependencyResolver(builder.Build());
        }

        private void ConfigureServiceProxies(ContainerBuilder builder, StatelessServiceContext context)
        {
            var serviceProxyFactory =
                new ServiceProxyFactory<IStatelessService1>(
                    new Uri($"fabric:/POC_Logging_CorrelationID/StatelessService1"));

            var serviceProxyFactoryQ =
                new ServiceProxyFactory<IReliableQueue1>(
                    new Uri($"fabric:/POC_Logging_CorrelationID/ReliableQueue"));

            builder.RegisterInstance(serviceProxyFactory)
                .As<IServiceProxyFactory<IStatelessService1>>()
                .SingleInstance();

            builder.RegisterInstance(serviceProxyFactoryQ)
               .As<IServiceProxyFactory<IReliableQueue1>>()
               .SingleInstance();

        }
    }
}
