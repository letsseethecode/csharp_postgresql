namespace LSTC.CheeseShop.Domain
{
    public class Product : Entity
    {
        public string Name { get; protected internal set; }
        public string Description { get; protected internal set; }
    }
}
