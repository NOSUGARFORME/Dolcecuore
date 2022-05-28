using Dolcecuore.Application.Common.Commands;
using Dolcecuore.Domain.Events;
using Dolcecuore.Services.Basket.Repositories.Interfaces;
using Dolcecuore.Services.Basket.Services;

namespace Dolcecuore.Services.Basket.Commands;

public record AddUpdateBasketCommand(Entities.Basket Basket) : ICommand;

public class AddUpdateBasketCommandHandler : ICommandHandler<AddUpdateBasketCommand>
{
    private readonly IDomainEvents _domainEvents;
    private readonly IDiscountGrpcService _discountGrpcService;

    public AddUpdateBasketCommandHandler(IDomainEvents domainEvents, IDiscountGrpcService discountGrpcService)
    {
        _domainEvents = domainEvents;
        _discountGrpcService = discountGrpcService;
    }

    public async Task HandleAsync(AddUpdateBasketCommand command, CancellationToken cancellationToken = default)
    {
        foreach (var item in command.Basket.Items)
        {
            var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
            item.Price -= coupon.Amount;
        }
        
        await _domainEvents.DispatchAsync(new EntityUpdatedEvent<Entities.Basket>(command.Basket, DateTime.UtcNow), cancellationToken);
    }
}
