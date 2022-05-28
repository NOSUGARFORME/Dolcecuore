namespace Dolcecuore.Services.Basket.Repositories.Interfaces
{
    public interface IBasketRepository
    {
        Task<Entities.Basket> GetBasket(string userName);
        Task UpdateBasket(Entities.Basket basket);
        Task DeleteBasket(Entities.Basket basket);
    }
}