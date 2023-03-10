using Backend.Libs.Domain;
using MediatR;

namespace Backend.Libs.Application.Commands.Abstractions;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}