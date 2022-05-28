using System;
using System.Threading.Tasks;
using Dolcecuore.CrossCuttingConcerns.Exceptions;
using Dolcecuore.Domain.Events;
using Dolcecuore.Services.Basket.Api.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Dolcecuore.Services.Basket.Api.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _cache;
        private readonly IDomainEvents _domainEvents;

        public BasketRepository(IDistributedCache cache, IDomainEvents domainEvents)
        {
            _cache = cache ?? throw new ArgumentException(null, nameof(cache));
            _domainEvents = domainEvents ?? throw new ArgumentException(null, nameof(domainEvents));
        }

        public async Task<Entities.Basket> GetBasket(string userName)
        {
            ValidationException.Requires(string.IsNullOrWhiteSpace(userName), "Invalid user name");
            var cart = await _cache.GetStringAsync(userName);
            return string.IsNullOrWhiteSpace(cart) ? null : JsonConvert.DeserializeObject<Entities.Basket>(cart);
        }

        public async Task UpdateBasket(Entities.Basket basket)
        {
            await _cache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));
            await _domainEvents.DispatchAsync(new EntityUpdatedEvent<Entities.Basket>(basket, DateTime.UtcNow));
        }

        public async Task DeleteBasket(Entities.Basket basket)
        {
            await _cache.RemoveAsync(basket.UserName);
            await _domainEvents.DispatchAsync(new EntityDeletedEvent<Entities.Basket>(basket, DateTime.UtcNow));
        }
    }
}