namespace LSTC.CheeseShop.Domain
{
    public class Product
    {
        public Guid Id { get; protected internal set; }
        public string Name { get; protected internal set; }
        public string Description { get; protected internal set; }
    }
}
