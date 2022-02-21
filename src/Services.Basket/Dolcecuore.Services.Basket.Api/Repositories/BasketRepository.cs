using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dolcecuore.Services.Basket.Api.Entities;
using Dolcecuore.Services.Basket.Api.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Dolcecuore.Services.Basket.Api.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _cache;

        public BasketRepository(IDistributedCache cache)
        {
            _cache = cache ?? throw new ArgumentException(null, nameof(cache));
        }

        public async Task<Entities.Basket> GetBasket(string userName)
        {
            var cart = await _cache.GetStringAsync(userName);
            return string.IsNullOrWhiteSpace(cart) ? null : JsonConvert.DeserializeObject<Entities.Basket>(cart);
        }

        public async Task<Entities.Basket> UpdateBasket(Entities.Basket basket)
        {
            await _cache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));
            return await GetBasket(basket.UserName);
        }

        public async Task DeleteBasket(string userName)
        {
            await _cache.RemoveAsync(userName);
        }
    }
}