using Microsoft.Extensions.DependencyInjection;

namespace LSTC.Shared.CQS.Queries;

public class ServiceProviderQueryResolver : IQueryResolver
{
    private IServiceProvider _serviceProvider;

    public ServiceProviderQueryResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IQueryHandler<TQuery> Resolve<TQuery>()
        where TQuery : IQueryResults
    {
        var processor = _serviceProvider.GetServices(typeof(IQueryHandler<TQuery>)) as IEnumerable<IQueryHandler<TQuery>>;
        var e = processor!.GetEnumerator();
        if (!e.MoveNext())
            throw new InvalidOperationException($"No query processor found for query type {typeof(TQuery).FullName}");
        var p = e.Current;
        if (e.MoveNext())
            throw new InvalidOperationException($"Multiple query processors found for query type {typeof(TQuery).FullName}");
        return p;
    }

    public IQueryHandler<TQuery, TArgs> Resolve<TQuery, TArgs>()
        where TQuery : IQueryResults
        where TArgs : IQueryArgs
    {
        var processor = _serviceProvider.GetServices(typeof(IQueryHandler<TQuery, TArgs>)) as IEnumerable<IQueryHandler<TQuery, TArgs>>;
        var e = processor!.GetEnumerator();
        if (!e.MoveNext())
            throw new InvalidOperationException($"No query processor found for query type {typeof(TQuery).FullName} with args of type {typeof(TArgs).FullName}");
        var p = e.Current;
        if (e.MoveNext())
            throw new InvalidOperationException($"Multiple query processors found for query type {typeof(TQuery).FullName} with args of type {typeof(TArgs).FullName}");
        return p;
    }
}