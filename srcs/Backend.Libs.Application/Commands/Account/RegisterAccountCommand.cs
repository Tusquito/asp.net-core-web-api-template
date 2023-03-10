using Backend.Libs.Application.Commands.Abstractions;

namespace Backend.Libs.Application.Commands.Account;

public sealed record RegisterAccountCommand(string Username, string Password, string Email, string Ip) : ICommand;
