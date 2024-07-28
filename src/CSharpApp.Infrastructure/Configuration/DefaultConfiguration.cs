using CSharpApp.Application.Services;
using CSharpApp.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpApp.Infrastructure.Configuration;

public static class DefaultConfiguration
{
    public static IServiceCollection AddDefaultConfiguration(this IServiceCollection services)
    {
        services.AddSingleton<ITodoService, TodoService>();
        services.AddSingleton<IPostService, PostService>();

        return services;
    }
}