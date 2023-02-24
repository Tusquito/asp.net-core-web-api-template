using MediatR;

namespace Backend.Libs.Mediator.Messaging.Abstractions;

public interface IRabbitMqMessage<T> : IRequest
{
}