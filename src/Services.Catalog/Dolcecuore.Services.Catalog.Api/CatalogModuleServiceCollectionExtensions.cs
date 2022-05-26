using System;
using System.Reflection;
using Dolcecuore.Application;
using Dolcecuore.Domain.Events;
using Dolcecuore.Domain.Repositories;
using Dolcecuore.Services.Catalog.Api.ConfigurationOptions;
using Dolcecuore.Services.Catalog.Api.Entities;
using Dolcecuore.Services.Catalog.Api.Repositories;
using Microsoft.AspNetCore.Builder;
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
            .AddScoped(typeof(IProductRepository), typeof(ProductRepository));

        DomainEvents.RegisterHandlers(Assembly.GetExecutingAssembly(), services);

        services.AddMessageHandlers(Assembly.GetExecutingAssembly());

        return services;
    }
    
    public static void MigrateProductDb(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
        serviceScope?.ServiceProvider.GetRequiredService<ProductDbContext>().Database.Migrate();
    }

    public static IServiceCollection AddHostedServicesProductModule(this IServiceCollection services)
    {
        return services;
    }
}