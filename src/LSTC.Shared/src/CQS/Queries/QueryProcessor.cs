using System.ComponentModel.DataAnnotations;
using LSTC.Shared.Domain;

namespace LSTC.Shared.CQS.Queries;

public class QueryProcessor : Processor
{
    private IQueryResolver _resolver;

    public QueryProcessor(IQueryResolver resolver)
    {
        _resolver = resolver;
    }

    public async Task<TResults> ExecuteAsync<TResults>()
        where TResults : IQueryResults
    {
        var processor = _resolver.Resolve<TResults>();
        return await processor.ExecuteAsync();
    }

    public async Task<TResults> ExecuteAsync<TResults, TArgs>(TArgs args)
        where TResults : IQueryResults
        where TArgs : IQueryArgs
    {
        var processor = _resolver.Resolve<TResults, TArgs>();
        Validate(args);
        return await processor.ExecuteAsync(args);
    }

    public void Validate<TArgs>(TArgs args) where TArgs : IQueryArgs
    {
        ValidateObject(args);
    }
}
