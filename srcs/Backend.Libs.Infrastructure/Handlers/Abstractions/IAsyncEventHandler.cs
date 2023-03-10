using Backend.Libs.Infrastructure.Messages.Abstractions;
using MediatR;

namespace Backend.Libs.Infrastructure.Handlers.Abstractions;

public interface IAsyncEventHandler<in T> : IRequestHandler<T> where T : IMessage
{
}