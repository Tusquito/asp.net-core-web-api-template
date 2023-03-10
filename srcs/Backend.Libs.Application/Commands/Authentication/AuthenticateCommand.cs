using Backend.Libs.Application.Commands.Abstractions;
using Backend.Libs.Domain.Models.Authentication;

namespace Backend.Libs.Application.Commands.Authentication;

public sealed record AuthenticateCommand(string Login, string Password) : ICommand<TokenModel>;