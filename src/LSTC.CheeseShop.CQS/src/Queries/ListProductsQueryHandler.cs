using LSTC.CheeseShop.Messages.Queries;
using LSTC.Shared.CQS.Queries;

namespace LSTC.CheeseShop.CQS.Queries;

public class ListProductsQueryHandler : IQueryHandler<ListProductsQuery.Results, ListProductsQuery.Args>
{
    public ListProductsQueryHandler()
    {
    }

    public async Task<ListProductsQuery.Results> ExecuteAsync(ListProductsQuery.Args args)
    {
        return await Task.FromResult(new ListProductsQuery.Results
        {
            Args = args,
            Products = [
                new ListProductsQuery.Product
                {
                    Id = args.Id,
                    Name = "Test Product"
                }
            ]
        });
    }
}
