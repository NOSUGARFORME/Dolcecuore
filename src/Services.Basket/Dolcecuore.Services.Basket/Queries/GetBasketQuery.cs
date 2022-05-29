using Dolcecuore.Application.Common.Query;
using Dolcecuore.CrossCuttingConcerns.Exceptions;
using Dolcecuore.Services.Basket.Repositories.Interfaces;

namespace Dolcecuore.Services.Basket.Queries;

public record GetBasketQuery(string UserName, bool ThrowNotFoundIfNull) : IQuery<Entities.Basket>;

public class GetBasketQueryHandler : IQueryHandler<GetBasketQuery, Entities.Basket>
{
    private readonly IBasketRepository _basketRepository;

    public GetBasketQueryHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }

    public async Task<Entities.Basket> HandleAsync(GetBasketQuery query, CancellationToken cancellationToken = default)
    {
        var basket = await _basketRepository.GetBasket(query.UserName);
        if (basket is null && query.ThrowNotFoundIfNull)
        {
            throw new NotFoundException($"{query.UserName}'s basket is not found.");
        }

        return basket ?? new Entities.Basket(query.UserName);
    }
}
