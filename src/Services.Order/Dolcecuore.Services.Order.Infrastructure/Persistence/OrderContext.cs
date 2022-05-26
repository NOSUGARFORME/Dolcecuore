using Dolcecuore.Services.Order.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Dolcecuore.Services.Order.Infrastructure.Persistence;

public class OrderContext : DbContext
{
    public OrderContext(DbContextOptions<OrderContext> options)
        : base(options)
    {
    }

    public DbSet<Domain.Entities.Order> Orders { get; set; } = default!;

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entity in ChangeTracker.Entries<EntityBase>())
        {
            switch (entity.State)
            {
                case EntityState.Added:
                    entity.Entity.CreatedDate = DateTime.Now;
                    entity.Entity.CreatedBy = "none";
                    break;
                case EntityState.Modified:
                    entity.Entity.LastModifiedDate = DateTime.Now;
                    entity.Entity.LastModifiedBy = "none";
                    break;
            }
        }
        
        return base.SaveChangesAsync(cancellationToken);
    }
}
