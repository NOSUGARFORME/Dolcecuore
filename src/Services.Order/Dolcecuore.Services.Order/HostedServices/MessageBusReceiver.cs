using Dolcecuore.Application.Common;
using Dolcecuore.Domain.Infrastructure.MessageBrokers;
using Dolcecuore.Services.Order.Commands;
using Dolcecuore.Services.Order.DTOs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dolcecuore.Services.Order.HostedServices;

public class MessageBusReceiver : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMessageReceiver<BasketCheckedEvent> _orderCreatedEventReceiver;

    public MessageBusReceiver(
        IMessageReceiver<BasketCheckedEvent> orderCreatedEventReceiver, IServiceProvider serviceProvider)
    {
        _orderCreatedEventReceiver = orderCreatedEventReceiver;
        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _orderCreatedEventReceiver.Receive(async (data, metaData) =>
        {
            using var scope = _serviceProvider.CreateScope();
            var dispatcher = scope.ServiceProvider.GetService<Dispatcher>()!;
            await dispatcher.DispatchAsync(new AddUpdateOrderCommand(data.Order), stoppingToken);
        });
        
        return Task.CompletedTask;
    }
}