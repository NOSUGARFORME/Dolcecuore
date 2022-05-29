using Dolcecuore.Infrastructure.MessageBrokers.RabbitMQ;

namespace Dolcecuore.Infrastructure.MessageBrokers;

public class MessageBrokerOptions
{
    public string Provider { get; set; }
    public RabbitMQOptions RabbitMQ { get; set; }

    public bool UsedRabbitMQ()
        => Provider == "RabbitMQ";
}