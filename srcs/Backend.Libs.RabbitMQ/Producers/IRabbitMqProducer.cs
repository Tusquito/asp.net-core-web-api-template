namespace Backend.Libs.RabbitMQ.Producers;

public interface IRabbitMqProducer<in T> where T : IRabbitMqMessage<T>
{
    Task PublishAsync(T message, CancellationToken cancellationToken = default);
}