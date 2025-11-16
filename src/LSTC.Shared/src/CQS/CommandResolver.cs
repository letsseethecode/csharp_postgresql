namespace LSTC.Shared.CQS;

public interface ICommandResolver
{
    /// <summary>
    /// Returns a command processor for the specified command type.
    /// </summary>
    /// <typeparam name="TCommand">The ICommand that is to be executed</typeparam>
    /// <exception cref="InvalidOperationException">If no processor or multiple processors are found.</exception>
    /// <returns></returns>
    public ICommandProcessor<TCommand> Resolve<TCommand>() where TCommand : ICommand;
}
