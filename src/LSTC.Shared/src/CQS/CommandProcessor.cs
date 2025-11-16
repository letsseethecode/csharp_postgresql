using System.Runtime.InteropServices.Swift;

namespace LSTC.Shared.CQS;

/// <summary>
/// Executes a single command
/// </summary>
/// <typeparam name="TCommand"></typeparam>
public interface ICommandProcessor<TCommand>
{
    /// <summary>
    /// Execute the command.
    /// </summary>
    /// <param name="command"></param>
    /// <returns>Returns a Task because it will be implemented as async</returns>
    public Task ExecuteAsync(TCommand command);
}

public class CommandProcessor
{
    private ICommandResolver _resolver;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="resolver">The resolve that will find command processors</param>
    public CommandProcessor(ICommandResolver resolver)
    {
        this._resolver = resolver;
    }

    /// <summary>
    /// Execute the command
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <param name="command"></param>
    public async void Execute<TCommand>(TCommand command) where TCommand : ICommand
    {
        var processor = this._resolver.Resolve<TCommand>();
        await processor.ExecuteAsync(command);
    }
}