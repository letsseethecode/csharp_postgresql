using LSTC.Shared.CQS.Queries;

namespace LSTC.Shared.CQS.Http;

public abstract class HttpQueryArgsMap<TQueryArgs> : HttpBaseMap<TQueryArgs>
    where TQueryArgs : IQueryArgs, new()
{
}
