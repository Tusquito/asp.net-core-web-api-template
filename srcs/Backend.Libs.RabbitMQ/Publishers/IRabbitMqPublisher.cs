namespace Backend.Libs.RabbitMQ.Publishers;

public interface IRabbitMqPublisher<in T> where T : IRabbitMqMessage<T>
{
    Task PublishAsync(T message, CancellationToken cancellationToken = default);
}