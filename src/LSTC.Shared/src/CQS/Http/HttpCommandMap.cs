using LSTC.Shared.CQS.Commands;

namespace LSTC.Shared.CQS.Http;

public abstract class HttpCommandMap<TCommand> : HttpBaseMap<TCommand>
    where TCommand : ICommand, new()
{
}
