namespace LSTC.Shared.CQS;

public interface IEventHandler<TEvent>
{
    Task HandleAsync(TEvent e);
}