namespace LSTC.CheeseShop.Domain
{
    public class Location
    {
        public Guid Id { get; protected internal set; }
        public string Name { get; protected internal set; }
        public string Description { get; protected internal set; }

        /// <summary>
        /// Indicates that the location is a Supplier and therefore does not 
        /// need to check stock levels before creating movements.
        /// </summary>
        public bool IsSupplier { get; protected internal set; }

        /// <summary>
        /// Indicates that this location is an external location, indicating
        /// that stock has been moved outside the business.
        /// </summary>
        public bool IsExternal { get; protected internal set; }

        public Movement? MoveProducts(IStockChecker stockChecker,
                                     Product product,
                                     Location destination,
                                     int quantity,
                                     DateTime date)
        {
            if (IsSupplier && !stockChecker.HasStock(this, product, quantity, date))
            {
                return null;
            }
            return new Movement
            {
                Id = Guid.NewGuid(),
                Product = product,
                Source = this,
                Destination = destination,
                Quantity = quantity,
                Date = date
            };
        }
    }
}