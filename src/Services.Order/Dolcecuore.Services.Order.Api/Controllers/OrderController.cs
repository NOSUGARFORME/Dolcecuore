using System.Net;
using Dolcecuore.Services.Order.Application.Features.Orders.Commands.CheckoutOrder;
using Dolcecuore.Services.Order.Application.Features.Orders.Commands.DeleteOrder;
using Dolcecuore.Services.Order.Application.Features.Orders.Commands.UpdateOrder;
using Dolcecuore.Services.Order.Application.Features.Orders.Queries.GetOrderList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dolcecuore.Services.Order.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet("{username}", Name = "GetOrders")]
    [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByUsername(string username)
    {
        var query = new GetOrdersListQuery(username);
        var orders = await _mediator.Send(query);
        
        return Ok(orders);
    }

    [HttpPost(Name = "CheckoutOrder")]
    [ProducesResponseType((int) HttpStatusCode.OK)]
    public async Task<ActionResult<int>> CheckoutOrder([FromBody] CheckoutOrderCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
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

    [HttpDelete("{id:int}", Name = "DeleteOrder")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> DeleteOrder(int id)
    {
        var command = new DeleteOrderCommand(id);
        await _mediator.Send(command);
        
        return NoContent();
    }
}