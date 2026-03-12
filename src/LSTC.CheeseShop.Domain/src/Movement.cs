namespace LSTC.CheeseShop.Domain
{
    public class Movement : Entity
    {
        public Location Source { get; set; }
        public Location Destination { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }

        public Movement(
            Guid id,
            Location source,
            Location destination,
            Product product,
            int quantity,
            DateTime date
        ) : base(id)
        {
            this.Source = source;
            this.Destination = destination;
            this.Product = product;
            this.Quantity = quantity;
            this.Date = date;
        }
    }
}
