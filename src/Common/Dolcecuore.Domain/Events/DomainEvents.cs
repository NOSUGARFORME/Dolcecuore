using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Dolcecuore.Domain.Events;

public class DomainEvents : IDomainEvents
{
    private static readonly List<Type> Handlers = new();
    private readonly IServiceProvider _serviceProvider;

    public static void RegisterHandlers(Assembly assembly, IServiceCollection services)
    {
        var types = assembly.GetTypes()
            .Where(x => x.GetInterfaces().Any(y =>
                y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)))
            .ToList();

        foreach (var type in types)
        {
            services.AddTransient(type);
        }

        Handlers.AddRange(types);
    }

    public DomainEvents(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        foreach (var handlerType in Handlers)
        {
            var canHandleEvent = handlerType.GetInterfaces()
                .Any(x => x.IsGenericType
                          && x.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)
                          && x.GenericTypeArguments[0] == domainEvent.GetType());

            if (!canHandleEvent) continue;
            dynamic handler = _serviceProvider.GetService(handlerType);
            await handler.HandleAsync((dynamic) domainEvent, cancellationToken);
        }
    }
}