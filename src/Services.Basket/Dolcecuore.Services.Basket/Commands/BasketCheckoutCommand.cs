using Dolcecuore.Application.Common.Commands;
using Dolcecuore.CrossCuttingConcerns.Exceptions;
using Dolcecuore.Domain.Events;
using Dolcecuore.Services.Basket.Entities;
using Dolcecuore.Services.Basket.Repositories.Interfaces;

namespace Dolcecuore.Services.Basket.Commands;

public record BasketCheckoutCommand(Order Order) : ICommand;

public class BasketCheckoutCommandHandler : ICommandHandler<BasketCheckoutCommand>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IDomainEvents _domainEvents;

    public BasketCheckoutCommandHandler(IBasketRepository basketRepository, IDomainEvents domainEvents)
    {
        _basketRepository = basketRepository;
        _domainEvents = domainEvents;
    }

    public async Task HandleAsync(BasketCheckoutCommand command, CancellationToken cancellationToken = default)
    {
        var basket = await _basketRepository.GetBasket(command.Order.UserName);
        if (basket is null)
        {
            throw new NotFoundException($"Basket by {command.Order.UserName} is not found.");
        }

        command.Order.TotalPrice = basket.Total;
        
        
        await _domainEvents.DispatchAsync(new EntityCreatedEvent<Order>(command.Order, DateTime.UtcNow), cancellationToken);
    }
}
