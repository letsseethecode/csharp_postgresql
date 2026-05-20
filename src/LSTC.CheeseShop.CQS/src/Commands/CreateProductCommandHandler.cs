using LSTC.CheeseShop.Domain;
using LSTC.CheeseShop.Messages.Commands;
using LSTC.Shared.CQS.Commands;

namespace LSTC.CheeseShop.CQS.Commands;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
{
    public CreateProductCommandHandler()
    {
    }

    public async Task ExecuteAsync(CreateProductCommand command)
    {
        var root = new Root();
        var result = root.CreateProduct(command.Id, command.Name, command.Description);
        await Task.CompletedTask;
    }
}
