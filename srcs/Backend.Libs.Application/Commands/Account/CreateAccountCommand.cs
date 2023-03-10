using Backend.Libs.Application.Commands.Abstractions;
using Backend.Libs.Persistence.Enums;

namespace Backend.Libs.Application.Commands.Account;

public sealed record CreateAccountCommand(string Username, string Password, string Email, List<RoleType> Roles, Guid Id = default) : ICommand;