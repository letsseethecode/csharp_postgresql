namespace LSTC.CheeseShop.Domain
{
    public class Location : Entity
    {
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

        public Location(
            Guid id,
            string name,
            string description,
            bool isSupplier = false,
            bool isExternal = false
        )
            : base(id)
        {
            this.Name = name;
            this.Description = description;
            this.IsSupplier = isSupplier;
            this.IsExternal = isExternal;
        }

        /// <summary>
        /// Create a movement of products from this location to a destination location.
        /// </summary>
        /// <param name="stockChecker">service that can check stock levelts</param>
        /// <param name="product">product to move</param>
        /// <param name="destination">destination location</param>
        /// <param name="quantity">number to move</param>
        /// <param name="date">when the movement is to occur</param>
        /// <returns></returns>
        public (Movement, DomainEvent)? MoveProducts(IStockChecker stockChecker,
                                                     Product product,
                                                     Location destination,
                                                     int quantity,
                                                     DateTime date
        )
        {
            if (IsSupplier && !stockChecker.HasStock(this, product, quantity, date))
            {
                return null;
            }

            var movement = new Movement(
                 Guid.NewGuid(),
                 this,
                 destination,
                 product,
                 quantity,
                 date
            );
            var @event = new MovementCreatedEvent(movement);

            return (movement, @event);
        }
    }
}
