namespace LSTC.Shared.CQS.Commands;

/// <summary>
/// Executes a single command
/// </summary>
/// <typeparam name="TCommand">The type of command to be executed</typeparam>
public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    /// <summary>
    /// Execute the command.
    /// </summary>
    /// <param name="command">The command to be executed</param>
    /// <returns>Returns a Task because it will be implemented as async</returns>
    public Task ExecuteAsync(TCommand command);
}
