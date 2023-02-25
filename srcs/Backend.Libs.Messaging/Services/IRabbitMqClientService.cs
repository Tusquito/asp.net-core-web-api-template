using RabbitMQ.Client;

namespace Backend.Libs.Messaging.Services;

public interface IRabbitMqClientService
{
    IModel GetChannel();
    IModel Get();
}