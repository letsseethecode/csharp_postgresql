using LSTC.CheeseShop.Messages.Queries;
using LSTC.Shared.CQS.Http;

namespace LSTC.CheeseShop.CQS.Queries;

public class ListProductsQueryHttpMap : HttpQueryArgsMap<ListProductsQuery.Args>
{
    public ListProductsQueryHttpMap()
    {
        Route("/products/{id}");
        FromPath(x => x.Id, "id");
    }
}
