using Dolcecuore.Application.Common.Query;
using Dolcecuore.Services.Order.Repositories;

namespace Dolcecuore.Services.Order.Queries;

public record GetOrdersByUserName(string UserName) : IQuery<List<Entities.Order>>;

public class GetOrdersByUserNameHandler : IQueryHandler<GetOrdersByUserName, List<Entities.Order>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersByUserNameHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public Task<List<Entities.Order>> HandleAsync(GetOrdersByUserName query,
        CancellationToken cancellationToken = default)
        => _orderRepository.GetOrdersByUsername(query.UserName);
}
