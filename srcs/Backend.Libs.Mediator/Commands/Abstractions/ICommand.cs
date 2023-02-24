using Backend.Libs.Domain;
using MediatR;

namespace Backend.Libs.Mediator.Commands.Abstractions;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}