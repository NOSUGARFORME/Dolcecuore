using System.Threading.Tasks;
using Dolcecuore.Services.Basket.Api.Entities;

namespace Dolcecuore.Services.Basket.Api.Repositories.Interfaces
{
    public interface IBasketRepository
    {
        Task<Entities.Basket> GetBasket(string userName);
        Task<Entities.Basket> UpdateBasket(Entities.Basket basket);
        Task DeleteBasket(string userName);
    }
}