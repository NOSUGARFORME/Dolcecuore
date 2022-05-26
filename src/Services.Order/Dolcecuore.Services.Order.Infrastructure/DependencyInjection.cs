using Dolcecuore.Services.Order.Application.Contracts.Infrastructure;
using Dolcecuore.Services.Order.Application.Contracts.Persistence;
using Dolcecuore.Services.Order.Application.Models;
using Dolcecuore.Services.Order.Infrastructure.Persistence;
using Dolcecuore.Services.Order.Infrastructure.Repositories;
using Dolcecuore.Services.Order.Infrastructure.Services.Mail;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dolcecuore.Services.Order.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>))
            .AddScoped<IOrderRepository, OrderRepository>()
            .Configure<EmailSettings>(c => configuration.GetSection("EmailSettings"))
            .AddTransient<IEmailService, EmailService>()
            .AddDbContext<OrderContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("OrderingConnectionString")));
}
