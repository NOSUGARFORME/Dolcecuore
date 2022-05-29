using System.Threading.Tasks;
using Dolcecuore.Application.Common;
using Dolcecuore.Services.Basket.Commands;
using Dolcecuore.Services.Basket.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dolcecuore.Services.Basket.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly Dispatcher _dispatcher;

        public BasketController(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
        
        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Basket.Entities.Basket>> GetBasket(string userName)
        {
            var basket = await _dispatcher.DispatchAsync(new GetBasketQuery(userName, false)); 
            return Ok(basket);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateBasket([FromBody] Basket.Entities.Basket basket)
        {
            await _dispatcher.DispatchAsync(new AddUpdateBasketCommand(basket));
            return Ok();
        }
        
        [HttpDelete("{userName}", Name = "DeleteBasket")]        
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _dispatcher.DispatchAsync(new DeleteBasketCommand(userName));
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
