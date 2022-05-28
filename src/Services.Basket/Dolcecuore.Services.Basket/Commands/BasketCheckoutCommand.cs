using Dolcecuore.Application.Common.Commands;
using Dolcecuore.CrossCuttingConcerns.Exceptions;
using Dolcecuore.Domain.Repositories;
using Dolcecuore.Services.Basket.Entities;
using Dolcecuore.Services.Basket.Repositories.Interfaces;

namespace Dolcecuore.Services.Basket.Commands;

public record BasketCheckoutCommand(string UserName) : ICommand;

public class BasketCheckoutCommandHandler : ICommandHandler<BasketCheckoutCommand>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IRepository<EventLog, long> _eventLogRepository;

    public BasketCheckoutCommandHandler(IBasketRepository basketRepository, IRepository<EventLog, long> eventLogRepository)
    {
        _basketRepository = basketRepository;
        _eventLogRepository = eventLogRepository;
    }

    public async Task HandleAsync(BasketCheckoutCommand command, CancellationToken cancellationToken = default)
    {
        var basket = await _basketRepository.GetBasket(command.UserName);
        if (basket is null)
        {
            throw new NotFoundException($"Basket by {command.UserName} is not found.");
        }
        
        await _basketRepository.DeleteBasket(basket);
    }
}
