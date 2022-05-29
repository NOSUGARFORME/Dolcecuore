using Dolcecuore.Application.Common.Commands;
using Dolcecuore.Application.Common.Services;

namespace Dolcecuore.Services.Order.Commands;

public record CheckoutOrderCommand(Entities.Order Order) : ICommand;

public class CheckoutOrderCommandHandler : ICommandHandler<CheckoutOrderCommand>
{
    private readonly ICrudService<Entities.Order> _orderService;

    public CheckoutOrderCommandHandler(ICrudService<Entities.Order> orderService)
    {
        _orderService = orderService;
    }

    public async Task HandleAsync(CheckoutOrderCommand command, CancellationToken cancellationToken = default)
    {
        await _orderService.AddOrUpdateAsync(command.Order, cancellationToken);
    }
}
