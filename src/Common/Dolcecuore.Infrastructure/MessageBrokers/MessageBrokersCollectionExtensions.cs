using Dolcecuore.Domain.Infrastructure.MessageBrokers;
using Dolcecuore.Infrastructure.MessageBrokers;
using Dolcecuore.Infrastructure.MessageBrokers.RabbitMQ;

namespace Microsoft.Extensions.DependencyInjection;

public static class MessageBrokersCollectionExtensions
{
    public static IServiceCollection AddRabbitMQSender<T>(this IServiceCollection services, RabbitMQOptions options)
        => services.AddSingleton<IMessageSender<T>>(new RabbitMQSender<T>(new RabbitMQSenderOptions
        {
            HostName = options.HostName,
            UserName = options.UserName,
            Password = options.Password,
            ExchangeName = options.ExchangeName,
            RoutingKey = options.RoutingKeys[typeof(T).Name],
        }));
    
    public static IServiceCollection AddRabbitMQReceiver<T>(this IServiceCollection services, RabbitMQOptions options)
        => services.AddTransient<IMessageReceiver<T>>(_ 
            => new RabbitMQReceiver<T>(new RabbitMQReceiverOptions
                {
                    HostName = options.HostName,
                    UserName = options.UserName,
                    Password = options.Password,
                    ExchangeName = options.ExchangeName,
                    RoutingKey = options.RoutingKeys[typeof(T).Name],
                    QueueName = options.QueueNames[typeof(T).Name],
                    AutomaticCreateEnabled = true,
                }));

    public static IServiceCollection AddMessageBusSender<T>(
        this IServiceCollection services,
        MessageBrokerOptions options)
    {
        if (options.UsedRabbitMQ())
        {
            services.AddRabbitMQSender<T>(options.RabbitMQ);

            // TODO: Add Health Check
        }

        return services;
    }
    
    public static IServiceCollection AddMessageBusReceiver<T>(this IServiceCollection services, MessageBrokerOptions options)
    {
        if (options.UsedRabbitMQ())
        {
            services.AddRabbitMQReceiver<T>(options.RabbitMQ);
        }

        return services;
    }
}