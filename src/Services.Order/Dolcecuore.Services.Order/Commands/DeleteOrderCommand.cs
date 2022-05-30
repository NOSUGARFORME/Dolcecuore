using Dolcecuore.Application.Common.Commands;
using Dolcecuore.Application.Common.Services;

namespace Dolcecuore.Services.Order.Commands;

public record DeleteOrderCommand(Entities.Order Order) : ICommand;

public class DeleteOrderCommandHandler : ICommandHandler<DeleteOrderCommand>
{
    private readonly ICrudService<Entities.Order> _orderService;

    public DeleteOrderCommandHandler(ICrudService<Entities.Order> orderService)
    {
        _orderService = orderService;
    }

    public async Task HandleAsync(DeleteOrderCommand command, CancellationToken cancellationToken = default)
    {
        await _orderService.DeleteAsync(command.Order, cancellationToken);
    }
}
