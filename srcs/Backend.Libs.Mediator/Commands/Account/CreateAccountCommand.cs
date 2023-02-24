using Backend.Libs.Database.Account;
using Backend.Libs.Mediator.Commands.Abstractions;

namespace Backend.Libs.Mediator.Commands.Account;

public sealed record CreateAccountCommand(string Username, string Password, string Email, List<RoleType> Roles, Guid Id = default) : ICommand;