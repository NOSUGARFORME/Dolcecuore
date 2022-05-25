using MediatR;

namespace Dolcecuore.Services.Order.Application.Features.Orders.Commands.UpdateOrder;

public record UpdateOrderCommand(
    int Id,
    string Username,
    decimal Price,
    string FirstName,
    string LastName,
    string EmailAddress,
    string AddressLine,
    string Country,
    string State,
    string ZipCode,
    int PaymentMethod) : IRequest;