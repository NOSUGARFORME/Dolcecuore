using System.Net;
using System.Threading.Tasks;
using Dolcecuore.Services.Basket.Api.GrpcServices;
using Dolcecuore.Services.Basket.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dolcecuore.Services.Basket.Api.Controllers
{
    [ApiController]
    [Route("api/v1[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly DiscountGrpcService _discountGrpcService;

        public BasketController(IBasketRepository basketRepository, DiscountGrpcService discountGrpcService)
        {
            _basketRepository = basketRepository;
            _discountGrpcService = discountGrpcService;
        }
        
        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(Entities.Basket), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Entities.Basket>> GetBasket(string userName)
        {
            var basket = await _basketRepository.GetBasket(userName);
            return Ok(basket ?? new Entities.Basket(userName));
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(Entities.Basket), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Entities.Basket>> UpdateBasket([FromBody] Entities.Basket basket)
        {
            foreach (var item in basket.Items)
            {
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }
        
            return Ok(await _basketRepository.UpdateBasket(basket));
        }
        
        [HttpDelete("{userName}", Name = "DeleteBasket")]        
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _basketRepository.DeleteBasket(userName);
            return Ok();
        }
        
    }
}