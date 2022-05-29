using Dolcecuore.Domain.Repositories;

namespace Dolcecuore.Services.Order.Repositories;

public interface IOrderRepository : IRepository<Entities.Order, Guid>
{
    Task<List<Entities.Order>> GetOrdersByUsername(string userName);
}