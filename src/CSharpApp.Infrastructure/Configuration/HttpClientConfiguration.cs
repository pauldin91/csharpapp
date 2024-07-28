using CSharpApp.Core.Config;
using CSharpApp.Core.Interfaces;
using CSharpApp.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

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