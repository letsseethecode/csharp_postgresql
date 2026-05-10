namespace LSTC.Shared.CQS.Events;

public interface IEventHandler<TEvent>
{
    Task HandleAsync(TEvent e);
}