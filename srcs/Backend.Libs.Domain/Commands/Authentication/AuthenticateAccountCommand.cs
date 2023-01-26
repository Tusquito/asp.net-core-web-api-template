using Backend.Libs.Domain.Abstractions;
using Backend.Libs.Models.Authentication;

namespace Backend.Libs.Domain.Commands.Authentication;

public sealed record AuthenticateAccountCommand(string Login, string Password) : ICommand<TokenModel>;