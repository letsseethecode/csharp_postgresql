using LSTC.Shared.CQS.Commands;
using LSTC.Shared.CQS.Http;

namespace LSTC.CheeseShop.Messages.Commands;

public class CreateProductCommand : ICommand
{
    public Guid? CorrelationId { get; set; } = null;

    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}
