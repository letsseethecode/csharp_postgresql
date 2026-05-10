namespace LSTC.Shared.CQS.Queries;

public interface IQueryResolver
{
    IQueryHandler<TQuery> Resolve<TQuery>()
        where TQuery : IQuery;

    IQueryHandler<TQuery, TArgs> Resolve<TQuery, TArgs>()
        where TQuery : IQuery;
}
