namespace LSTC.CheeseShop.Domain
{
    public class MovementCreatedEvent : LTSC.CheeseShop.CQS.DomainEvent
    {
        public Movement Movement { get; private set; }

        public MovementCreatedEvent(Movement movement)
        {
            this.Movement = movement;
        }
    }
}
