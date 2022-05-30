using Dolcecuore.Application.Common.Commands;
using Dolcecuore.Application.Common.Services;

namespace Dolcecuore.Services.Order.Commands;

public record AddUpdateOrderCommand(Entities.Order Order) : ICommand;

public class AddUpdateOrderCommandHandler : ICommandHandler<AddUpdateOrderCommand>
{
    private readonly ICrudService<Entities.Order> _orderService;

    public AddUpdateOrderCommandHandler(ICrudService<Entities.Order> orderService)
    {
        _orderService = orderService;
    }

    public async Task HandleAsync(AddUpdateOrderCommand command, CancellationToken cancellationToken = default)
    {
        await _orderService.AddOrUpdateAsync(command.Order, cancellationToken);
    }
}
