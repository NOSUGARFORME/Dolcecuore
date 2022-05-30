using Microsoft.EntityFrameworkCore;

namespace Dolcecuore.Services.Order.Repositories;

public class OrderRepository : Repository<Entities.Order, Guid>, IOrderRepository
{
    public OrderRepository(OrderDbContext dbContext) 
        : base(dbContext)
    {
    }

    public Task<List<Entities.Order>> GetOrdersByUsername(string userName)
        => GetAll()
            .Where(o => o.UserName == userName)
            .ToListAsync();
}