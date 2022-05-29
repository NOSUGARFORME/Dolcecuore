using Dolcecuore.Application.Common.Query;
using Dolcecuore.CrossCuttingConcerns.Exceptions;
using Dolcecuore.Services.Order.Repositories;

namespace Dolcecuore.Services.Order.Queries;

public record GetOrderQuery(Guid Id, bool ThrowNotFoundIfNull) : IQuery<Entities.Order>;

public class GetOrderQueryHandler : IQueryHandler<GetOrderQuery, Entities.Order>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Entities.Order> HandleAsync(GetOrderQuery query, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.FirstOrDefaultAsync(
            _orderRepository
                .GetAll()
                .Where(o => o.Id == query.Id));

        if (query.ThrowNotFoundIfNull && order is null)
        {
            throw new NotFoundException($"Order {query.Id} is not found.");
        }

        return order;
    }
}
