using Microsoft.Extensions.DependencyInjection;

namespace LSTC.Shared.CQS.Events;

public class ServiceProviderEventResolver : IEventResolver
{
    private readonly IServiceProvider _serviceProvider;

    public ServiceProviderEventResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IEnumerable<IEventHandler<TEvent>> Resolve<TEvent>()
        where TEvent : IEvent
    {
        var p = _serviceProvider.GetServices(typeof(IEventHandler<TEvent>)) as IEnumerable<IEventHandler<TEvent>>;
        return p!;
    }
}
