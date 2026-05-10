namespace LSTC.Shared.CQS.Queries;

public class QueryProcessor
{
    private IQueryResolver _resolver;

    public QueryProcessor(IQueryResolver resolver)
    {
        _resolver = resolver;
    }

    public async Task<TQuery> ExecuteAsync<TQuery>()
        where TQuery : IQuery
    {
        var processor = _resolver.Resolve<TQuery>();
        return await processor.ExecuteAsync();
    }

    public async Task<TQuery> ExecuteAsync<TQuery, TArgs>(TArgs args)
        where TQuery : IQuery
    {
        var processor = _resolver.Resolve<TQuery, TArgs>();
        return await processor.ExecuteAsync(args);
    }
}
