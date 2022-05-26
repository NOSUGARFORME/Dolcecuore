namespace Dolcecuore.Services.Order.Application.Contracts.Persistence;

public interface IOrderRepository : IRepository<Domain.Entities.Order>
{
    Task<List<Domain.Entities.Order>> GetOrdersByUsername(string username);
}
