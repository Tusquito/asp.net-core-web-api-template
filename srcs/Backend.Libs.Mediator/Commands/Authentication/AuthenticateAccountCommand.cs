using Backend.Libs.Mediator.Commands.Abstractions;
using Backend.Libs.Models.Authentication;

namespace Backend.Libs.Mediator.Commands.Authentication;

public sealed record AuthenticateAccountCommand(string Login, string Password) : ICommand<TokenModel>;