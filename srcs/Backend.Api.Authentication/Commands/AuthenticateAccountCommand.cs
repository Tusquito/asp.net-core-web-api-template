using Backend.Libs.Mediator.Commands.Abstractions;
using Backend.Libs.Models.Authentication;

namespace Backend.Api.Authentication.Commands;

public sealed record AuthenticateAccountCommand(string Login, string Password) : ICommand<TokenModel>;