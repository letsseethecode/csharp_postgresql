using LSTC.CheeseShop.Messages.Queries;
using LSTC.Shared.CQS.Http;

namespace LSTC.CheeseShop.CQS.Queries;

public class ListProductsQueryMap : HttpQueryArgsMap<ListProductsQuery.Args>
{
    public ListProductsQueryMap()
    {
        Route("/products/{id}");
        FromPath(x => x.Id, "id");
    }
}
