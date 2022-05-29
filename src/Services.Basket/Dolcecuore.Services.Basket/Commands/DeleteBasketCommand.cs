using Dolcecuore.Application.Common.Commands;
using Dolcecuore.CrossCuttingConcerns.Exceptions;
using Dolcecuore.Domain.Events;
using Dolcecuore.Services.Basket.Repositories.Interfaces;

namespace Dolcecuore.Services.Basket.Commands;

public record DeleteBasketCommand(string UserName) : ICommand;

public class DeleteBasketCommandHandler : ICommandHandler<DeleteBasketCommand>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IDomainEvents _domainEvents;

    public DeleteBasketCommandHandler(
        IBasketRepository basketRepository,
        IDomainEvents domainEvents)
    {
        _basketRepository = basketRepository;
        _domainEvents = domainEvents;
    }

    public async Task HandleAsync(DeleteBasketCommand command, CancellationToken cancellationToken = default)
    {
        var basket = await _basketRepository.GetBasket(command.UserName);
        if (basket is null)
        {
            throw new NotFoundException($"Basket by {command.UserName} is not found.");
        }
        
        //TODO: handle transaction
        await _basketRepository.DeleteBasket(basket);
        await _domainEvents.DispatchAsync(new EntityDeletedEvent<Entities.Basket>(basket, DateTime.UtcNow), cancellationToken);
    }
}
