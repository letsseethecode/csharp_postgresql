using LSTC.CheeseShop.Messages.Queries;
using LSTC.Shared.CQS.Queries;

namespace LSTC.CheeseShop.CQS.Queries;

public class ListProductsQueryHandler : IQueryHandler<ListProductsQuery.Results, ListProductsQuery.Args>
{
    public Task<ListProductsQuery.Results> ExecuteAsync(ListProductsQuery.Args args)
    {
        return Task.FromResult(new ListProductsQuery.Results
        {
            Args = args,
            Data = $"{{ \"Id\": \"{args.Id}\", \"Name\": \"Test Product\" }}"
        });
    }
}