using Backend.Libs.Domain.Abstractions;

namespace Backend.Libs.Domain.Commands.Authentication;

public sealed record RegisterAccountCommand(string Username, string Password, string PasswordConfirmation, string Email) : ICommand;
