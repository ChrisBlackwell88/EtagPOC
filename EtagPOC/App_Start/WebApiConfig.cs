using System.Linq;
using CacheCow.Server;
using System.Web.Http;

namespace EtagPOC
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Etag Cachecow.server
            GlobalConfiguration.Configuration.MessageHandlers.Add(new CachingHandler(GlobalConfiguration.Configuration));

            // swagger API versioning
            config.AddApiVersioning(apiVersioningOptions =>
            {
                apiVersioningOptions.ReportApiVersions = true;
                apiVersioningOptions.AssumeDefaultVersionWhenUnspecified = true;
            });

            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Default to JSON by removing XML support
            var appXmlType =
                config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
            //// config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Indent return values
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

            // Use camelCase
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
        }
    }
}
