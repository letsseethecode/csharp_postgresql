namespace LSTC.Shared.CQS.Commands;

/// <summary>
/// Resolves the command handler registered for any given command.
/// </summary>
public interface ICommandResolver
{
    /// <summary>
    /// Returns a command processor for the specified command type.
    /// </summary>
    /// <typeparam name="TCommand">The ICommand that is to be executed</typeparam>
    /// <exception cref="InvalidOperationException">If no processor or multiple processors are found.</exception>
    /// <returns>The command handler for the specified command type.</returns>
    public ICommandHandler<TCommand> Resolve<TCommand>() where TCommand : ICommand;
}
