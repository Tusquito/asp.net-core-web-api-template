using MediatR;

namespace Backend.Libs.Mediator.Messaging.Abstractions;

public interface IAsyncRabbitMqEventHandler<in T> : IRequestHandler<T> where T : IRabbitMqMessage<T>, IRequest
{
}