using LSTC.Shared.CQS;

namespace LSTC.CheeseShop.Messages.Commands;

public class CreateProductCommand : ICommand
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }
}