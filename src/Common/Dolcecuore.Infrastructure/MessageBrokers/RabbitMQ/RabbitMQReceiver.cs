using System.Text;
using System.Text.Json;
using Dolcecuore.Domain.Infrastructure.MessageBrokers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Dolcecuore.Infrastructure.MessageBrokers.RabbitMQ;

public class RabbitMQReceiver<T> : IMessageReceiver<T>, IDisposable
{
    private readonly IConnection _connection;
    private IModel _channel;
    private readonly string _queueName;
    private readonly RabbitMQReceiverOptions _options;
    
    public RabbitMQReceiver(RabbitMQReceiverOptions options)
    {
        _options = options;

        _connection = new ConnectionFactory
        {
            HostName = options.HostName,
            UserName = options.UserName,
            Password = options.Password,
            AutomaticRecoveryEnabled = true,
        }.CreateConnection();

        _queueName = options.QueueName;

        _connection.ConnectionShutdown += Connection_ConnectionShutdown;
    }
    
    private void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
    {
        // TODO: add log here
    }
    
    public void Receive(Action<T, MetaData> action)
    {
        _channel = _connection.CreateModel();

        if (_options.AutomaticCreateEnabled)
        {
            _channel.QueueDeclare(_options.QueueName, true, false, false, null);
            _channel.QueueBind(_options.QueueName, _options.ExchangeName, _options.RoutingKey, null);
        }

        _channel.BasicQos(0, 1, false);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (_, ea) =>
        {
            var body = Encoding.UTF8.GetString(ea.Body.Span);
            var message = JsonSerializer.Deserialize<Message<T>>(body)!;
            action(message.Data, message.MetaData);
            _channel.BasicAck(ea.DeliveryTag, false);
        };
        
        _channel.BasicConsume(_queueName, false, consumer);
    }
    
    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}