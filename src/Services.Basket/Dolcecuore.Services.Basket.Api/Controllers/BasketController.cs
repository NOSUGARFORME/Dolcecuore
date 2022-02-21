using System.Net;
using System.Threading.Tasks;
using Dolcecuore.Services.Basket.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dolcecuore.Services.Basket.Api.Controllers
{
    [ApiController]
    [Route("api/v1[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
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