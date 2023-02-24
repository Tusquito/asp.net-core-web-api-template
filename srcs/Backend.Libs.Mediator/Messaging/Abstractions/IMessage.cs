using MediatR;

namespace Backend.Libs.Mediator.Messaging.Abstractions;

public interface IMessage<T> : IRequest
{
}