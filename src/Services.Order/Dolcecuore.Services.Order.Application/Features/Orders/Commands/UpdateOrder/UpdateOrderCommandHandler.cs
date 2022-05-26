using AutoMapper;
using Dolcecuore.Services.Order.Application.Contracts.Persistence;
using Dolcecuore.Services.Order.Application.Exceptions;
using Dolcecuore.Services.Order.Application.Features.Orders.Commands.CheckoutOrder;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dolcecuore.Services.Order.Application.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateOrderCommandHandler> _logger;
    
    public UpdateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper,
        ILogger<UpdateOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.Id);
        if (order is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Order), request.Id);

        }

        _mapper.Map(request, order, typeof(UpdateOrderCommand), typeof(Domain.Entities.Order));

        await _orderRepository.UpdateAsync(order);
        
        _logger.LogInformation($"Order {order.Id} is successfully updated.");
        
        return Unit.Value;
    }
}