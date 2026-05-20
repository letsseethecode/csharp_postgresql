using System.Reflection;
using LSTC.Shared.CQS.Commands;
using LSTC.Shared.CQS.Http;
using LSTC.Shared.CQS.Queries;

namespace Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddCommandMaps(this IServiceCollection services, Assembly a)
    {
        var mapTypes = a.GetTypes()
            .Where(t => 
                t is { IsClass: true, IsAbstract: false } && 
                t.BaseType is { IsGenericType: true } &&
                t.BaseType.GetGenericTypeDefinition() == typeof(HttpCommandMap<>)
            ).ToArray();

        foreach (var mapType in mapTypes)
        {
            var commandType = mapType.BaseType!.GetGenericArguments()[0];
            var genericMapType = typeof(HttpCommandMap<>).MakeGenericType(commandType);
            services.AddTransient(genericMapType, mapType);
        }
        return services;
    }

    public static IServiceCollection AddCommandHandlers(this IServiceCollection services, Assembly a)
    {
        var handlerTypes = a.GetTypes()
            .Where(t => 
                t is { IsClass: true, IsAbstract: false } && 
                t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>))
            ).ToArray();


        foreach (var handlerType in handlerTypes)
        {
            var commandType = handlerType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)).GetGenericArguments()[0];
            var genericHandlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);
            services.AddTransient(genericHandlerType, handlerType);
        }
        return services;
    }

    public static IServiceCollection AddQueryMaps(this IServiceCollection services, Assembly a)
    {
        var mapTypes = a.GetTypes()
            .Where(t => 
                t is { IsClass: true, IsAbstract: false } && 
                t.BaseType is { IsGenericType: true } &&
                t.BaseType.GetGenericTypeDefinition() == typeof(HttpQueryArgsMap<>)
            ).ToArray();

        foreach (var mapType in mapTypes)
        {
            var queryArgsType = mapType.BaseType!.GetGenericArguments()[0];
            var genericMapType = typeof(HttpQueryArgsMap<>).MakeGenericType(queryArgsType);
            services.AddTransient(genericMapType, mapType);
        }
        return services;
    }

    public static IServiceCollection AddQueryHandlers(this IServiceCollection services, Assembly a)
    {
        var handlerTypes = a.GetTypes()
            .Where(t => 
                t is { IsClass: true, IsAbstract: false } && 
                t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))
            ).ToArray();

        foreach (var handlerType in handlerTypes)
        {
            var args = handlerType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)).GetGenericArguments();
            var queryType = args[0];
            var resultType = args[1];
            var genericHandlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, resultType);
            services.AddTransient(genericHandlerType, handlerType);
        }
        return services;
    }
}