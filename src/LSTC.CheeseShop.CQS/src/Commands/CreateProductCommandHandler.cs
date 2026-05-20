using LSTC.Shared.Data;
using LSTC.CheeseShop.Domain;
using LSTC.CheeseShop.Messages.Commands;
using LSTC.Shared.CQS.Commands;

namespace LSTC.CheeseShop.CQS.Commands;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
{
    private IRepository<Product, Guid> _repository;

    public CreateProductCommandHandler()
    {
    }

    public Task ExecuteAsync(CreateProductCommand command)
    {
        var root = new Root();
        var result = root.CreateProduct(command.Id, command.Name, command.Description);
        return Task.CompletedTask;
    }
}
