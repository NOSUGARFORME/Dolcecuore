using System.Reflection;
using Dolcecuore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Dolcecuore.Services.Catalog.Api.Repositories;

public class ProductDbContext : DbContextUnitOfWork<ProductDbContext>
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}