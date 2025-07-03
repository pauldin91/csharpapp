using CSharpApp.Core.Config;
using CSharpApp.Infrastructure.Services;
using CSharpApp.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpApp.Infrastructure.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureExensions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ITodoService, TodoService>();
        services.AddSingleton<IPostService, PostService>();
        var allClientSettings = typeof(PostSettings).Assembly.GetTypes()
            .Where(s => typeof(ClientSettings).IsAssignableFrom(s) && !s.IsAbstract);

        foreach (var type in allClientSettings)
        {
            var settings = (ClientSettings)Activator.CreateInstance(type)!;
            configuration.Bind(type.Name, settings);
            services.AddSingleton(type, settings);
        }
        services.AddSingleton<IHttpClientWrapper, HttpClientWrapper>();
        return services;
    }
}