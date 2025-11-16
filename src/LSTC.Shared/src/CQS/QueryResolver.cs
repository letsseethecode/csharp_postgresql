namespace LSTC.Shared.CQS;

public interface IQueryResolver
{
    Task<IQueryProcessor<TQuery>> Resolve<TQuery>() where TQuery : IQuery;
    Task<IQueryProcessor<TQuery, TArgs>> Resolve<TQuery, TArgs>() where TQuery : IQuery;
}