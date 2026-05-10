namespace LSTC.Shared.CQS.Events;

public interface IEventResolver
{
    IEnumerable<IEventHandler<TEvent>> Resolve<TEvent>()
        where TEvent : IEvent;
}
