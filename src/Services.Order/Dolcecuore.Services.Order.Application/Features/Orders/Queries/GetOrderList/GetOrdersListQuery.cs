using MediatR;

namespace Dolcecuore.Services.Order.Application.Features.Orders.Queries.GetOrderList;

public record GetOrdersListQuery(string Username) : IRequest<List<OrderDto>>
{
}