namespace LTSC.CheeseShop.CQS
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
