
using System.Reflection;
using Dolcecuore.Application.Common;
using Dolcecuore.Application.Common.Commands;
using Dolcecuore.Application.Common.Query;
using Dolcecuore.Application.Common.Services;
using Dolcecuore.Domain.Entities;
using Dolcecuore.Domain.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Dolcecuore.Application;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<Dispatcher>();

        services
            .AddScoped<IDomainEvents, DomainEvents>()
            .AddScoped(typeof(ICrudService<>), typeof(CrudService<>));

        return services;
    }
    
    public static IServiceCollection AddMessageHandlers(this IServiceCollection services, Assembly assembly)
    {
        var assemblyTypes = assembly.GetTypes();

        foreach (var type in assemblyTypes)
        {
            var handlerInterfaces = type.GetInterfaces()
                .Where(Utils.IsHandlerInterface)
                .ToList();

            if (!handlerInterfaces.Any()) continue;
            
            var handlerFactory = new HandlerFactory(type);
            foreach (var interfaceType in handlerInterfaces)
            {
                services.AddTransient(interfaceType, provider => handlerFactory.Create(provider, interfaceType));
            }
        }

        var aggregateRootTypes = assembly.GetTypes().Where(x => x.BaseType == typeof(AggregateRoot<Guid>)).ToList();

        var genericHandlerTypes = new[]
        {
            typeof(GetEntitiesQueryHandler<>),
            typeof(GetEntityByIdQueryHandler<>),
            typeof(AddOrUpdateEntityCommandHandler<>),
            typeof(DeleteEntityCommandHandler<>)
        };

        foreach (var aggregateRootType in aggregateRootTypes)
        {
            foreach (var genericHandlerType in genericHandlerTypes)
            {
                var handlerType = genericHandlerType.MakeGenericType(aggregateRootType);
                var handlerInterfaces = handlerType.GetInterfaces();

                var handlerFactory = new HandlerFactory(handlerType);
                foreach (var interfaceType in handlerInterfaces)
                {
                    services.AddTransient(interfaceType, provider => handlerFactory.Create(provider, interfaceType));
                }
            }
        }

        return services;
    }
}