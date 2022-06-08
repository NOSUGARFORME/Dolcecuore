using System;
using System.Reflection;
using Dolcecuore.Domain.Events;
using Dolcecuore.Domain.Repositories;
using Dolcecuore.Infrastructure.FullTextSearch;
using Dolcecuore.Infrastructure.Identity;
using Dolcecuore.Services.Catalog.Api.ConfigurationOptions;
using Dolcecuore.Services.Catalog.Api.DTOs;
using Dolcecuore.Services.Catalog.Api.Entities;
using Dolcecuore.Services.Catalog.Api.HostedServices;
using Dolcecuore.Services.Catalog.Api.Models;
using Dolcecuore.Services.Catalog.Api.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dolcecuore.Services.Catalog.Api;

public static class CatalogModuleServiceCollectionExtensions
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddDbContext<ProductDbContext>(options => options.UseSqlServer(appSettings.ConnectionStrings.Dolcecuore, sql =>
        {
            if (!string.IsNullOrEmpty(appSettings.ConnectionStrings.MigrationsAssembly))
            {
                sql.MigrationsAssembly(appSettings.ConnectionStrings.MigrationsAssembly);
            }
        }));

        services
            .AddScoped<IRepository<Product, Guid>, Repository<Product, Guid>>()
            .AddScoped(typeof(IProductRepository), typeof(ProductRepository))
            .AddScoped<IRepository<AuditLogEntry, Guid>, Repository<AuditLogEntry, Guid>>()
            .AddScoped<IRepository<EventLog, long>, Repository<EventLog, long>>();

        DomainEvents.RegisterHandlers(Assembly.GetExecutingAssembly(), services);

        services.AddMessageHandlers(Assembly.GetExecutingAssembly());

        services.AddMessageBusSender<AuditLogCreatedEvent>(appSettings.MessageBroker);
        
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<ICurrentUser, CurrentUser>();

        services.AddFullTextSearch<ProductFullTextModel>(appSettings.Elasticsearch);
        
        return services;
    }
    
    public static void MigrateProductDb(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
        serviceScope?.ServiceProvider.GetRequiredService<ProductDbContext>().Database.Migrate();
    }

    public static IServiceCollection AddHostedServicesCatalogModule(this IServiceCollection services)
    {
        services
            .AddScoped<PublishEventService>()
            .AddHostedService<PublishEventWorker>();
        
        return services;
    }
}