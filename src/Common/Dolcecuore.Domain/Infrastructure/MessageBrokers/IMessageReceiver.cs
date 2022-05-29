namespace Dolcecuore.Domain.Infrastructure.MessageBrokers;

public interface IMessageReceiver<out T>
{
    void Receive(Action<T, MetaData> action);
}