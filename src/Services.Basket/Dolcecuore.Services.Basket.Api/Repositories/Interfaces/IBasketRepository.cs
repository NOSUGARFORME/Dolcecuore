using System.Threading.Tasks;
using Dolcecuore.Services.Basket.Api.Entities;

namespace Dolcecuore.Services.Basket.Api.Repositories.Interfaces
{
    public interface IBasketRepository
    {
        Task<Entities.Basket> GetBasket(string userName);
        Task UpdateBasket(Entities.Basket basket);
        Task DeleteBasket(Entities.Basket basket);
    }
}