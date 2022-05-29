namespace Dolcecuore.Domain.Infrastructure.MessageBrokers;

public interface IMessageSender<in T>
{
    Task SendAsync(T message, MetaData metaData = null, CancellationToken cancellationToken = default);
}