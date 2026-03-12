namespace LSTC.CheeseShop.Domain
{
    public class DomainEvent
    {
        public DateTime OccurredOn { get; private set; }

        public DomainEvent()
        {
            this.OccurredOn = DateTime.UtcNow;
        }
    }
}
