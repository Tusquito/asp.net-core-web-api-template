using Backend.Libs.Database.Account;
using Backend.Libs.Mediator.Commands.Abstractions;

namespace Backend.Api.Tests.Commands;

public sealed record CreateAccountCommand(string Username, string Password, string Email, List<RoleType> Roles, Guid Id = default) : ICommand;