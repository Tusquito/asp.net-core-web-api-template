using Backend.Libs.Mediator.Commands.Abstractions;

namespace Backend.Libs.Mediator.Commands.Authentication;

public sealed record RegisterAccountCommand(string Username, string Password, string Email, string Ip) : ICommand;
