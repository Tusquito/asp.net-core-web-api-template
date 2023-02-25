using Backend.Libs.Mediator.Messaging.Abstractions;

namespace Backend.Libs.Messaging.Abstractions;

public interface IMessageProducer<T> where T : IMessage
{
    Task ProduceAsync(T message, CancellationToken cancellationToken = default);
    Task ProduceAsync(List<T> messages, CancellationToken cancellationToken = default);
}