using System.Reflection;
using Dolcecuore.Application;
using Dolcecuore.Domain.Events;
using Dolcecuore.Domain.Repositories;
using Dolcecuore.Infrastructure.Caching;
using Dolcecuore.Infrastructure.Identity;
using Dolcecuore.Services.Basket.ConfigurationOptions;
using Dolcecuore.Services.Basket.DTOs;
using Dolcecuore.Services.Basket.Entities;
using Dolcecuore.Services.Basket.HostedServices;
using Dolcecuore.Services.Basket.Repositories;
using Dolcecuore.Services.Basket.Repositories.Interfaces;
using Dolcecuore.Services.Basket.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dolcecuore.Services.Basket;

public static class BasketModuleServiceCollectionExtensions
{
    public static IServiceCollection AddBasketModule(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddDbContext<BasketEventDbContext>(options => options.UseSqlServer(appSettings.ConnectionStrings.Dolcecuore, sql =>
        {
            if (!string.IsNullOrEmpty(appSettings.ConnectionStrings.MigrationsAssembly))
            {
                sql.MigrationsAssembly(appSettings.ConnectionStrings.MigrationsAssembly);
            }
        }));
        
        services
            .AddScoped<IDiscountGrpcService, DiscountGrpcService>()
            .AddScoped(typeof(IBasketRepository), typeof(BasketRepository))
            .AddScoped<IRepository<AuditLogEntry, Guid>, Repository<AuditLogEntry, Guid>>()
            .AddScoped<IRepository<EventLog, long>, Repository<EventLog, long>>();
        
        services.AddCaches(appSettings.Caching);
        
        DomainEvents.RegisterHandlers(Assembly.GetExecutingAssembly(), services);
        
        services.AddMessageHandlers(Assembly.GetExecutingAssembly());
        
        services
            .AddMessageBusSender<BasketUpdatedEvent>(appSettings.MessageBroker)
            .AddMessageBusSender<BasketDeletedEvent>(appSettings.MessageBroker)
            .AddMessageBusSender<BasketCheckedEvent>(appSettings.MessageBroker)
            .AddMessageBusSender<AuditLogCreatedEvent>(appSettings.MessageBroker);
        
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<ICurrentUser, CurrentUser>();

        return services;
    }

    public static void MigrateBasketEventDb(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
        serviceScope?.ServiceProvider.GetRequiredService<BasketEventDbContext>().Database.Migrate();
    }

    public static IServiceCollection AddHostedServicesBasketModule(this IServiceCollection services)
    {
        services
            .AddScoped<PublishEventService>()
            .AddHostedService<PublishEventWorker>();

        return services;
    }
}