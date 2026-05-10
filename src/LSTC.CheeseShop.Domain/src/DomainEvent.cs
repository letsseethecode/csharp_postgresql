using LSTC.Shared.CQS.Events;

namespace LSTC.CheeseShop.Domain
{
    public class DomainEvent : IEvent
    {
        public DateTime OccurredOn { get; private set; }

        public DomainEvent()
        {
            this.OccurredOn = DateTime.UtcNow;
        }
    }
}
