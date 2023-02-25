using MediatR;

namespace Backend.Libs.Mediator.Messaging.Abstractions;

public interface IAsyncEventHandler<in T> : IRequestHandler<T> where T : IMessage
{
}