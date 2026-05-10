namespace LSTC.Shared.CQS.Queries;

public interface IQueryResolver
{
    IQueryHandler<TQuery> Resolve<TQuery>()
        where TQuery : IQueryResults;

    IQueryHandler<TQuery, TArgs> Resolve<TQuery, TArgs>()
        where TQuery : IQueryResults
        where TArgs : IQueryArgs;
}
