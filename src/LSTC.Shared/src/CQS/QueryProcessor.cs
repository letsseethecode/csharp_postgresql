namespace LSTC.Shared.CQS;

public interface IQueryProcessor<TQuery>
{
    Task<TQuery> ExecuteAsync();
}

public interface IQueryProcessor<TQuery, TArgs>
{
    Task<TQuery> ExecuteAsync(TArgs args);
}

public class QueryProcessor
{
    private IQueryResolver _resolver;

    public QueryProcessor(IQueryResolver resolver)
    {
        this._resolver = resolver;
    }

    public async Task<TQuery> ExecuteAsync<TQuery>()
        where TQuery : IQuery
    {
        var processor = await this._resolver.Resolve<TQuery>();
        return await processor.ExecuteAsync();
    }

    public async Task<TQuery> ExecuteAsync<TQuery, TArgs>(TArgs args)
        where TQuery : IQuery
    {
        var processor = await this._resolver.Resolve<TQuery, TArgs>();
        return await processor.ExecuteAsync(args);
    }
}
