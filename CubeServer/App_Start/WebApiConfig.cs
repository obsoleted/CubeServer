namespace CubeServer
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Web.Http;
    using System.Web.Http.ModelBinding;
    using Autofac;
    using Autofac.Integration.WebApi;
    using Data;
    using Microsoft.AspNet.WebApi.MessageHandlers.Compression;
    using Microsoft.AspNet.WebApi.MessageHandlers.Compression.Compressors;
    using Microsoft.WindowsAzure;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
           config.MessageHandlers.Insert(0, new ServerCompressionHandler(new GZipCompressor(), new DeflateCompressor()));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            SetupDependency(config);
        }

        private static void SetupDependency(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterInstance<IModelRepository>(new AzureModelRepository());

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}