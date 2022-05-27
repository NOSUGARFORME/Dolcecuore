using System.Threading;
using System.Threading.Tasks;
using Dolcecuore.Application.Common.Commands;
using Dolcecuore.CrossCuttingConcerns.Exceptions;
using Dolcecuore.Services.Basket.Api.Repositories.Interfaces;

namespace Dolcecuore.Services.Basket.Api.Commands;

public record BasketCheckoutCommand(string UserName) : ICommand;

public class BasketCheckoutCommandHandler : ICommandHandler<BasketCheckoutCommand>
{
    private readonly IBasketRepository _basketRepository;

    public BasketCheckoutCommandHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }

    public async Task HandleAsync(BasketCheckoutCommand command, CancellationToken cancellationToken = default)
    {
        var basket = await _basketRepository.GetBasket(command.UserName);
        if (basket is null)
        {
            throw new NotFoundException($"Basket by {command.UserName} is not found.");
        }
        
        await _basketRepository.DeleteBasket(basket.UserName);
    }
}
