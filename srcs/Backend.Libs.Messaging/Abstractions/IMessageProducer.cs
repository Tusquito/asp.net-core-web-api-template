using Backend.Libs.Mediator.Messaging.Abstractions;

namespace Backend.Libs.Messaging.Abstractions;

public interface IMessageProducer
{
    Task ProduceAsync<T>(T message, CancellationToken cancellationToken = default) where T : IMessage;
    Task ProduceAsync<T>(List<T> messages, CancellationToken cancellationToken = default) where T : IMessage;
}