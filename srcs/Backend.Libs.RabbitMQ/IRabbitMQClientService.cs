using RabbitMQ.Client;

namespace Backend.Libs.RabbitMQ;

public interface IRabbitMQClientService
{
    IModel GetChannel();
    IModel Get();
}