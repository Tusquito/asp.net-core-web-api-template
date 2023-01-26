using RabbitMQ.Client;

namespace Backend.Libs.RabbitMQ;

public interface IRabbitMqClientService
{
    IModel GetChannel();
    IModel Get();
}