using Backend.Libs.Mediator.Commands.Abstractions;

namespace Backend.Api.Authentication.Commands;

public sealed record RegisterAccountCommand(string Username, string Password, string Email, string Ip) : ICommand;
