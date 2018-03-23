using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using WebActivatorEx;
using EtagPOC;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace EtagPOC
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
            });

            GlobalConfiguration.Configuration
                .EnableSwagger(swaggerDocsConfig =>
                {
                    swaggerDocsConfig.UseFullTypeNameInSchemaIds();
                    swaggerDocsConfig.MultipleApiVersions(
                        (apiDesc, targetApiVersion) => ResolveApiVersion(apiDesc, targetApiVersion),
                        (vc) =>
                        {                           
                            vc.Version("1-0", "EtagPOC UI v1.0");
                        });
                    swaggerDocsConfig.ResolveConflictingActions(apiDescriptions =>
                    {
                        return apiDescriptions.First();
                    });

                    swaggerDocsConfig.IncludeXmlComments(string.Format(@"{0}\bin\EtagPOC.XML", System.AppDomain.CurrentDomain.BaseDirectory));
                })
                .EnableSwaggerUi(swaggerUiConfig =>
                {
                    swaggerUiConfig.EnableDiscoveryUrlSelector();
                });
        }

        public static bool ResolveApiVersion(ApiDescription apiDesc, string targetApiVersion)
        {
            var versionedApiDescription = apiDesc as Microsoft.Web.Http.Description.VersionedApiDescription;

            if (targetApiVersion.Replace("-", ".") == versionedApiDescription.ApiVersion.ToString())
            {
                return true;
            }

            return false;
        }
    }
}
