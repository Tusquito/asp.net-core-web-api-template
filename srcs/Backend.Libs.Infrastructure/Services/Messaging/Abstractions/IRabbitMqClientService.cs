using RabbitMQ.Client;

namespace Backend.Libs.Infrastructure.Services.Messaging.Abstractions;

public interface IRabbitMqClientService
{
    IModel GetChannel();
    IModel Get();
}