using CSharpApp.Api.Configuration.Routes;

namespace CSharpApp.Api.Configuration
{
    public static class WebApplicationConfiguration
    {
        public static WebApplication AddWebApplicationConfiguration(this WebApplication app)
        {
            var namespc = typeof(PostRouteConfiguration).Namespace;
            var types = typeof(PostRouteConfiguration).Assembly.GetTypes();
            var routeConfigs = types.Where(s => !string.IsNullOrEmpty(s.Namespace) && s.Namespace.Equals(namespc) && !s.Name.StartsWith("<"));
            foreach (var routeConfig in routeConfigs)
            {
                var cls = routeConfig.GetMethods().FirstOrDefault(s => s.Name.EndsWith("Routes"));
                cls.Invoke(null, new object[] { app });
            }
            return app;
        }
    }
}