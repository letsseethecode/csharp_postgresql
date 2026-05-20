using LSTC.Shared.CQS.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using LSTC.Shared.CQS.Http;
using Microsoft.AspNetCore.Routing;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.OpenApi;
using LSTC.Shared.Http;
using System.Threading.Tasks;
using LSTC.Shared.CQS.Queries;

namespace Microsoft.AspNetCore.Builder;

public static class EndpointRegistrationExtensions
  {
    /// <summary>
    /// Turns exceptions into IResult of ApiResponse with appropriate status
    /// codes and error messages.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    private static async Task<IResult?> TrapErrors(Func<Task> action)
    {
        try
        {
            await action();
            return null;
        }
        catch (Exception ex) when (ex is FormatException or ValidationException)
        {
            return Results.BadRequest(
                new ApiResponse("Bad Request")
                    .AddError(ex.Message, ex.StackTrace)
            );
        }
        catch (AggregateException ex) when (ex.InnerExceptions.All(e => e is ValidationException))
        {
            var errors = ex.InnerExceptions
                .Select(e => new ApiResponse.Error(e.Message, e.StackTrace))
                .ToArray();
            return Results.BadRequest(
                new ApiResponse("Bad Request", errors)
            );
        }
        catch (InvalidOperationException ex)
        {
            return Results.UnprocessableEntity(
                new ApiResponse("Unprocessable Entity")
                    .AddError(ex.Message, ex.StackTrace)
            );
        }
        catch (Exception ex)
        {
            return Results.InternalServerError(
                new ApiResponse("Internal Server Error")
                    .AddError(ex.Message, ex.StackTrace)
            );
        }
    }

    private static void MapCommandEndpoint<TCommand>(
        HttpCommandMap<TCommand> map,
        ICommandHandler<TCommand> handler,
        WebApplication app
    ) where TCommand : class, ICommand, new()
    {
        app.MapPost(map.Path, async (HttpContext context) =>
        {
            return await TrapErrors(async () =>
            {
                var command = await map.CreateAsync(context.Request, context.GetRouteData().Values);
                await handler.ExecuteAsync(command);
            }) ?? Results.Ok(new ApiResponse("OK"));
        })
        .WithOpenApi(op => OpenApiHelper.Generate(op, map))
        .Produces<ApiResponse>(StatusCodes.Status200OK)
        .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse>(StatusCodes.Status422UnprocessableEntity)
        .Produces<ApiResponse>(StatusCodes.Status500InternalServerError);
    }

    private static void MapEndpointForCommand<TCommand>(WebApplication app)
        where TCommand : class, ICommand, new()
    {
        var map = app.Services.GetService<HttpCommandMap<TCommand>>()!;
        var handler = app.Services.GetService<ICommandHandler<TCommand>>()!;
        MapCommandEndpoint(map, handler, app);
    }

    public static WebApplication MapEndpointsForCommands(this WebApplication app)
    {
        var mapCommandEndpoint = typeof(EndpointRegistrationExtensions)
            .GetMethod(nameof(MapEndpointForCommand), BindingFlags.NonPublic | BindingFlags.Static)!;
        var collection = app.Services.GetService<IServiceCollection>()!;
        foreach (var descriptor in collection.Where(d => d.ServiceType.IsGenericType && d.ServiceType.GetGenericTypeDefinition() == typeof(ICommandHandler<>)))
        {
            var commandType = descriptor.ServiceType.GetGenericArguments()[0];
            var genericMapCommandEndpoint = mapCommandEndpoint.MakeGenericMethod(commandType);
            genericMapCommandEndpoint.Invoke(null, [app]);
        }
        return app;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Queries
    // -----------------------------------------------------------------------------------------------------------------

    private static void MapQueryEndpoint<TQueryResults, TQueryArgs>(
        HttpQueryArgsMap<TQueryArgs> map,
        IQueryHandler<TQueryResults, TQueryArgs> handler,
        WebApplication app
    ) where TQueryResults : class, IQueryResults, new()
      where TQueryArgs : class, IQueryArgs, new()
    {
        app.MapGet(map.Path, async (HttpContext context) =>
        {
            TQueryResults? result = default;
            return await TrapErrors(async () =>
            {
                var args = await map.CreateAsync(context.Request, context.GetRouteData().Values);
                result = await handler.ExecuteAsync(args);
            }) ?? Results.Ok(new ApiResponse<TQueryResults>("OK", result));
        })
        .WithOpenApi(op => OpenApiHelper.Generate(op, map))
        .Produces<ApiResponse<TQueryResults>>(StatusCodes.Status200OK)
        .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse>(StatusCodes.Status422UnprocessableEntity)
        .Produces<ApiResponse>(StatusCodes.Status500InternalServerError);
    }

    private static void MapEndpointForQuery<TQueryResults, TQueryArgs>(WebApplication app)
        where TQueryResults : class, IQueryResults, new()
        where TQueryArgs : class, IQueryArgs, new()
    {
        var map = app.Services.GetService<HttpQueryArgsMap<TQueryArgs>>()!;
        var handler = app.Services.GetService<IQueryHandler<TQueryResults, TQueryArgs>>()!;
        MapQueryEndpoint(map, handler, app);
    }

    public static WebApplication MapEndpointsForQueries(this WebApplication app) {
        var mapQueryEndpoint = typeof(EndpointRegistrationExtensions)
            .GetMethod(nameof(MapEndpointForQuery), BindingFlags.NonPublic | BindingFlags.Static)!;
        var collection = app.Services.GetService<IServiceCollection>()!;
        foreach (var descriptor in collection.Where(d => d.ServiceType.IsGenericType && d.ServiceType.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)))
        {
            var args = descriptor.ServiceType.GetGenericArguments();
            var resultsType = args[0];
            var argsType = args[1];
            var genericMapQueryEndpoint = mapQueryEndpoint.MakeGenericMethod(resultsType, argsType);
            genericMapQueryEndpoint.Invoke(null, [app]);
        }
        return app;
    }
}
