namespace LSTC.CheeseShop.Domain
{
    public class Entity
    {
        public Guid Id { get; protected internal set; }

        public Entity(Guid id)
        {
            this.Id = id;
        }
    }
}
