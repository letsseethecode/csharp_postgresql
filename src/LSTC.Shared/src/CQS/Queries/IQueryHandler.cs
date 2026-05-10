namespace LSTC.Shared.CQS.Queries;

public interface IQueryHandler<TQuery>
{
    Task<TQuery> ExecuteAsync();
}

public interface IQueryHandler<TQuery, TArgs>
{
    Task<TQuery> ExecuteAsync(TArgs args);
}
