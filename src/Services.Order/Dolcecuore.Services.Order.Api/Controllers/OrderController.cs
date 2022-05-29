using System.Net;
using AutoMapper;
using Dolcecuore.Application.Common;
using Dolcecuore.Services.Order.Api.Models;
using Dolcecuore.Services.Order.Application.Features.Orders.Commands.UpdateOrder;
using Dolcecuore.Services.Order.Application.Features.Orders.Queries.GetOrderList;
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
    [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByUsername(string username)
    {
        var query = new GetOrdersListQuery(username);
        var orders = await _mediator.Send(query);
        
        return Ok(orders);
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
    public async Task<IActionResult> CheckoutOrder([FromBody] CheckoutOrderCommand command)
    {
        await _dispatcher.DispatchAsync(command);
        return Ok();
    }

    [HttpPut(Name = "UpdateOrder")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderCommand command)
    {
        await _mediator.Send(command);
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