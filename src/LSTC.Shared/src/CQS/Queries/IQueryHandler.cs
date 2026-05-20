namespace LSTC.Shared.CQS.Queries;

public interface IQueryHandler<TResults, TArgs>
    where TResults : IQueryResults
    where TArgs : IQueryArgs
{
    Task<TResults> ExecuteAsync(TArgs args);
}
