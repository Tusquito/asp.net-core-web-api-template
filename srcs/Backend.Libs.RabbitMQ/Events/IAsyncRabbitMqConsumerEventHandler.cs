using RabbitMQ.Client.Events;

namespace Backend.Libs.RabbitMQ.Events;

public interface IAsyncRabbitMqConsumerEventHandler<T> where T : IRabbitMqMessage<T>
{
    Task HandleAsync(object sender, BasicDeliverEventArgs @event);
}