using MediatR;

namespace Dolcecuore.Services.Order.Application.Features.Orders.Commands.DeleteOrder;

public record DeleteOrderCommand(int Id) : IRequest
{
}