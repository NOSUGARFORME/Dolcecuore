using System.Reflection;
using Dolcecuore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Dolcecuore.Services.Order.Repositories;

public class OrderDbContext : DbContextUnitOfWork<OrderDbContext>
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) 
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}