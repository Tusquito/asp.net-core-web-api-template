namespace Backend.Libs.RabbitMQ.Consumers;

public interface IRabbitMqConsumer<in T>  where T : IRabbitMqMessage<T>
{
}