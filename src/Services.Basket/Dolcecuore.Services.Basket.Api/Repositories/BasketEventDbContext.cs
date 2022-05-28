using System.Reflection;
using Dolcecuore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Dolcecuore.Services.Basket.Api.Repositories;

public class BasketEventDbContext : DbContextUnitOfWork<BasketEventDbContext>
{
    public BasketEventDbContext(DbContextOptions<BasketEventDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}