using AutoMapper;
using Dolcecuore.Application.Common;
using Dolcecuore.Services.Order.Api.Models;
using Dolcecuore.Services.Order.Commands;
using Dolcecuore.Services.Order.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Dolcecuore.Services.Order.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class OrderController : ControllerBase
{
    private readonly Dispatcher _dispatcher;
    private readonly IMapper _mapper;

    public OrderController(Dispatcher dispatcher, IMapper mapper)
    {
        _dispatcher = dispatcher;
        _mapper = mapper;
    }

    [HttpGet("{username}", Name = "GetOrders")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<OrderModel>>> GetOrdersByUsername(string userName)
    {
        var orders = await _dispatcher.DispatchAsync(new GetOrdersByUserName(userName));
        var models = orders.Select(o => _mapper.Map<OrderModel>(o));
        
        return Ok(models);
    }

    [HttpGet("{id:guid}", Name = "GetOrder")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<OrderModel>> GetOrder(Guid id)
    {
        var order = await _dispatcher.DispatchAsync(new GetOrderQuery(id, true));
        return Ok(_mapper.Map<OrderModel>(order));
    }
    
    [HttpPost(Name = "CheckoutOrder")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckoutOrder([FromBody] OrderModel model)
    {
        var order = _mapper.Map<Entities.Order>(model);
        await _dispatcher.DispatchAsync(new AddUpdateOrderCommand(order));
        return Ok();
    }

    [HttpPut("{id:guid}",Name = "UpdateOrder")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> UpdateOrder(Guid id, [FromBody] OrderModel model)
    {
        var order = await _dispatcher.DispatchAsync(new GetOrderQuery(id, true));
        order = _mapper.Map<Entities.Order>(order);
        
        await _dispatcher.DispatchAsync(new AddUpdateOrderCommand(order));
        return NoContent();
    }

    [HttpDelete("{id:guid}", Name = "DeleteOrder")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> DeleteOrder(Guid id)
    {
        var order = await _dispatcher.DispatchAsync(new GetOrderQuery(id, true));
        await _dispatcher.DispatchAsync(new DeleteOrderCommand(order));
        return NoContent();
    }
}