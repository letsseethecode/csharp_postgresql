namespace LSTC.CheeseShop.CQS.Commands
{
    using LSTC.Shared.CQS;
    using LSTC.Shared.Data;
    using LSTC.CheeseShop.Domain;
    using LSTC.CheeseShop.Messages;

    public class CreateProductCommandProcessor : ICommandProcessor<CreateProductCommand>
    {
        private IRepository<Product, Guid> _repository;

        public CreateProductCommandProcessor(IRepository<Product, Guid> repository)
        {
            this._repository = repository;
        }

        public async Task ExecuteAsync(CreateProductCommand command)
        {
            var root = new Root();
            var product = root.CreateProduct(command.Id, command.Name, command.Description);
            await this._repository.Save(product);
        }
    }
}