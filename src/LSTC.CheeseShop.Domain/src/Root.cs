using LSTC.Shared.Domain;

namespace LSTC.CheeseShop.Domain
{
    /// <summary>
    /// There is a rule that only Domain entities can create Domain entities.
    ///
    /// Therefore entities with no clear parent will be instantiated by this class.
    /// </summary>
    public class Root
    {
        public Result<Product> CreateProduct(Guid id, string name, string description)
        {
            return Result.Success(new Product(id, name, description));
        }

        public Result<Location> CreateLocation(Guid id, string name, string description)
        {
            return Result.Success(new Location(id, name, description));
        }
    }
}
