namespace LSTC.CheeseShop.Domain
{
    /// <summary>
    /// There is a rule that only Domain entities can create Domain entities.
    ///
    /// Therefore entities with no clear parent will be instantiated by this class.
    /// </summary>
    public class Root
    {
        public Product CreateProduct(Guid id, string name, string description)
        {
            return new Product
            {
                Id = id,
                Name = name,
                Description = description
            };
        }

        public Location CreateLocation(Guid id, string name, string description)
        {
            return new Location
            {
                Id = id,
                Name = name,
                Description = description,
            };
        }
    }
}