namespace Backend.Libs.RabbitMQ.Handlers;

public interface IAsyncRabbitMqConsumerMessageHandler<in T> where T : IRabbitMqMessage<T>
{
    Task HandleAsync(T message, CancellationToken cancellationToken);
}