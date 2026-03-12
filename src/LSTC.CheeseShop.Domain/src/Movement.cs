namespace LSTC.CheeseShop.Domain
{
    public class Movement : Entity
    {
        public Location Source { get; set; }
        public Location Destination { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
    }
}