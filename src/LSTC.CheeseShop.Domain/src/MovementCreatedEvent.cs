namespace LSTC.CheeseShop.Domain
{
    public class MovementCreatedEvent : DomainEvent
    {
        public Movement Movement { get; private set; }

        public MovementCreatedEvent(Movement movement)
        {
            this.Movement = movement;
        }
    }
}
