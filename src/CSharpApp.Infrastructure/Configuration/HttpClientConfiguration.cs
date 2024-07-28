using CSharpApp.Core;
using CSharpApp.Core.Config;
using CSharpApp.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpApp.Infrastructure.Configuration
{
    public static class HttpClientConfiguration
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            var allClientSettings = typeof(PostSettings).Assembly.GetTypes()
                .Where(s => typeof(IClientSettings).IsAssignableFrom(s) && !s.IsAbstract);

            foreach (var type in allClientSettings)
            {
                var settings = (IClientSettings)Activator.CreateInstance(type)!;
                configuration.Bind(type.Name, settings);
                services.AddSingleton(type, settings);
                services.AddHttpClient(type.Name, cfg =>
                {
                    cfg.BaseAddress = new Uri(settings.BaseUrl);
                });
                var genericIfcType = typeof(IHttpClientWrapper<>).MakeGenericType(type);
                var genericImplType = typeof(HttpClientWrapper<>).MakeGenericType(type);
                services.AddSingleton(genericIfcType, genericImplType);
            }

            return services;
        }
    }
}