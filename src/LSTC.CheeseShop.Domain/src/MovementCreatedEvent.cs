namespace LSTC.CheeseShop.Domain
{
    public class MovementCreatedEvent : DomainEvent
    {
        public Movement Movement { get; }

        public MovementCreatedEvent(Movement movement)
        {
            Movement = movement;
        }
    }
}
