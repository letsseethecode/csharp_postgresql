using Microsoft.Extensions.DependencyInjection;

namespace LSTC.Shared.CQS.Commands;

/// <summary>
/// Implementation of ICommandResolver that uses an IServiceProvider.
/// </summary>
public class ServiceProviderCommandResolver : ICommandResolver
{
    private readonly IServiceProvider _serviceProvider;

    public ServiceProviderCommandResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ICommandHandler<TCommand> Resolve<TCommand>()
        where TCommand : ICommand
    {
        var processor = _serviceProvider.GetServices(typeof(ICommandHandler<TCommand>)) as IEnumerable<ICommandHandler<TCommand>>;
        var e = processor!.GetEnumerator();
        if (!e.MoveNext())
            throw new InvalidOperationException($"No command processor found for command type {typeof(TCommand).FullName}");
        var p = e.Current;
        if (e.MoveNext())
            throw new InvalidOperationException($"Multiple command processors found for command type {typeof(TCommand).FullName}");
        return p;
    }
}