using LSTC.CheeseShop.Messages.Commands;
using LSTC.Shared.CQS.Http;

namespace LSTC.CheeseShop.CQS.Commands;

public class CreateProductCommandMap : HttpCommandMap<CreateProductCommand>
{
    public CreateProductCommandMap()
    {
        Route("/product");
        FromHeader(x => x.CorrelationId, "X-Correlation-Id");
        FromBody(x => x.Id);
        FromBody(x => x.Name);
        FromBody(x => x.Description);
    }
}
