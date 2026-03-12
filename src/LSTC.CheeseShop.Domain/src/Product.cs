namespace LSTC.CheeseShop.Domain
{
    public class Product : Entity
    {
        public string Name { get; protected internal set; }
        public string Description { get; protected internal set; }

        public Product(Guid id, string name, string description) : base(id)
        {
            this.Name = name;
            this.Description = description;
        }
    }
}
