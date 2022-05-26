using Dolcecuore.Services.Order.Application.Contracts.Persistence;
using Dolcecuore.Services.Order.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Dolcecuore.Services.Order.Infrastructure.Repositories;

public class OrderRepository : RepositoryBase<Domain.Entities.Order>, IOrderRepository
{
    public OrderRepository(OrderContext context) : base(context)
    {
    }

    public Task<List<Domain.Entities.Order>> GetOrdersByUsername(string username)
        => Context.Orders
            .Where(o => o.Username == username)
            .ToListAsync();
}
