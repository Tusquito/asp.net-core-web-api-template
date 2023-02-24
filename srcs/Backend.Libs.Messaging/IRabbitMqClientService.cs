using RabbitMQ.Client;

namespace Backend.Libs.Messaging;

public interface IRabbitMqClientService
{
    IModel GetChannel();
    IModel Get();
}