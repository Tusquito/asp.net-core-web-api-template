using Backend.Libs.Mediator.Messaging.Abstractions;

namespace Backend.Libs.Messaging.Consumers;

public interface IRabbitMqConsumer<in T> where T : IRabbitMqMessage<T>
{
}