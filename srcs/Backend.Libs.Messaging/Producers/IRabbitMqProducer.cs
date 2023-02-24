using Backend.Libs.Mediator.Messaging.Abstractions;

namespace Backend.Libs.Messaging.Producers;

public interface IRabbitMqProducer<in T> where T : IRabbitMqMessage<T>
{
    Task ProduceAsync(T message, CancellationToken cancellationToken = default);
}