using System.Net;
using System.Threading.Tasks;
using Dolcecuore.Application.Common;
using Dolcecuore.Services.Basket.Commands;
using Dolcecuore.Services.Basket.Repositories.Interfaces;
using Dolcecuore.Services.Basket.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dolcecuore.Services.Basket.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly DiscountGrpcService _discountGrpcService;
        private readonly Dispatcher _dispatcher;

        public BasketController(
            IBasketRepository basketRepository,
            DiscountGrpcService discountGrpcService,
            Dispatcher dispatcher)
        {
            _basketRepository = basketRepository;
            _discountGrpcService = discountGrpcService;
            _dispatcher = dispatcher;
        }
        
        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(Basket.Entities.Basket), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Basket.Entities.Basket>> GetBasket(string userName)
        {
            var basket = await _basketRepository.GetBasket(userName);
            return Ok(basket ?? new Basket.Entities.Basket(userName));
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(Basket.Entities.Basket), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Basket.Entities.Basket>> UpdateBasket([FromBody] Basket.Entities.Basket basket)
        {
            foreach (var item in basket.Items)
            {
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }

            return Ok();
            // return Ok(await _basketRepository.UpdateBasket(basket));
        }
        
        [HttpDelete("{userName}", Name = "DeleteBasket")]        
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            // await _basketRepository.DeleteBasket(userName);
            return NoContent();
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckoutCommand basketCheckout)
        {
            await _dispatcher.DispatchAsync(basketCheckout);
            return Accepted();
        }
    }
}
