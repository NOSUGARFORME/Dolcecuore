namespace Dolcecuore.Services.Order.Application.Contracts.Persistence;

public interface IOrderRepository : IRepository<Domain.Entities.Order>
{
    Task<IEnumerable<Domain.Entities.Order>> GetOrdersByUsername(string username);
}