namespace LSTC.Shared.CQS.Commands;

/// <summary>
/// The CommandProcessor is responsible for executing commands.
/// </summary>
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
    public async Task ExecuteAsync<TCommand>(TCommand command) where TCommand : ICommand
    {
        var processor = this._resolver.Resolve<TCommand>();
        await processor.ExecuteAsync(command);
    }
}