using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace LSTC.Shared.Tests;

public static class ServiceCollectionExtensions
{
    private static void CheckAddScoped<TType>(IServiceCollection services, Type type)
    {
        if (type is { IsClass: true, IsAbstract: false } && typeof(TType).IsAssignableFrom(type))
        {
            services.AddScoped(typeof(TType), type);
        }
    }

    private static void CheckAddSingleton<TType>(IServiceCollection services, Type type)
    {
        if (type is { IsClass: true, IsAbstract: false } && typeof(TType).IsAssignableFrom(type))
        {
            services.AddSingleton(typeof(TType), type);
        }
    }

    public static IServiceCollection AddScopedFromAssembly<TType>(this IServiceCollection services, Assembly assembly)
    {
        var types = assembly.GetTypes();
        foreach (var type in types)
        {
            CheckAddScoped<TType>(services, type);
        }
        return services;
    }

    public static IServiceCollection AddSingletonFromAssembly<TType>(this IServiceCollection services, Assembly assembly)
    {
        var types = assembly.GetTypes();
        foreach (var type in types)
        {
            CheckAddSingleton<TType>(services, type);
        }
        return services;
    }
}
