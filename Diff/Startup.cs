using Diff.Ioc;
using Microsoft.Owin;
using Microsoft.Web.Http.Routing;
using Owin;
using Swashbuckle.Application;
using System.Web.Http;
using System.Web.Http.Routing;

[assembly: OwinStartup(typeof(Diff.Startup))]

namespace Diff
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            var constraintResolver = new DefaultInlineConstraintResolver()
            {
                ConstraintMap =
                {
                    ["apiVersion"] = typeof(ApiVersionRouteConstraint )
                }
            };
            config.MapHttpAttributeRoutes(constraintResolver);

            config.AddApiVersioning();

            app.UseWebApi(config);

            config
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Diff API - Andrew Signori");
                    c.IncludeXmlComments($"{System.AppDomain.CurrentDomain.BaseDirectory}\\App_Data\\Diff.XML");
                })
                .EnableSwaggerUi(c => c.DocExpansion(DocExpansion.List));

            config.DependencyResolver = DependencyRegistrations.GetDependencyResolver();
        }
    }
}
