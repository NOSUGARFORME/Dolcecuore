using System.Reflection;
using Dolcecuore.Domain.Events;
using Dolcecuore.Domain.Repositories;
using Dolcecuore.Infrastructure.Identity;
using Dolcecuore.Services.Order.ConfigureOptions;
using Dolcecuore.Services.Order.DTOs;
using Dolcecuore.Services.Order.Entities;
using Dolcecuore.Services.Order.HostedServices;
using Dolcecuore.Services.Order.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dolcecuore.Services.Order;

public static class OrderModuleServiceCollectionExtensions
{
    public static IServiceCollection AddOrderModule(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddDbContext<OrderDbContext>(options => options.UseSqlServer(appSettings.ConnectionStrings.Dolcecuore, sql =>
        {
            if (!string.IsNullOrEmpty(appSettings.ConnectionStrings.MigrationsAssembly))
            {
                sql.MigrationsAssembly(appSettings.ConnectionStrings.MigrationsAssembly);
            }
        }));
        
        services
            .AddScoped<IRepository<Entities.Order, Guid>, Repository<Entities.Order, Guid>>()
            .AddScoped(typeof(IOrderRepository), typeof(OrderRepository))
            .AddScoped<IRepository<AuditLogEntry, Guid>, Repository<AuditLogEntry, Guid>>()
            .AddScoped<IRepository<EventLog, long>, Repository<EventLog, long>>();

        DomainEvents.RegisterHandlers(Assembly.GetExecutingAssembly(), services);

        services.AddMessageHandlers(Assembly.GetExecutingAssembly());
        
        services
            .AddMessageBusSender<AuditLogCreatedEvent>(appSettings.MessageBroker)
            .AddMessageBusSender<OrderCreatedEvent>(appSettings.MessageBroker)
            .AddMessageBusReceiver<BasketCheckedEvent>(appSettings.MessageBroker);

        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<ICurrentUser, CurrentUser>();

        return services;
    }
    
    public static void MigrateOrderDb(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
        serviceScope?.ServiceProvider.GetRequiredService<OrderDbContext>().Database.Migrate();
    }

    public static IServiceCollection AddHostedServicesOrderModule(this IServiceCollection services)
        => services
            .AddHostedService<MessageBusReceiver>()
            .AddHostedService<PublishEventWorker>()
            .AddScoped<PublishEventService>();
}
