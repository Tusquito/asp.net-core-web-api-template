using System;
using System.Collections.Generic;
using Backend.Libs.Database.Account;
using Backend.Libs.Domain.Abstractions;

namespace Backend.Libs.Domain.Commands.Account;

public sealed record CreateAccountCommand(string Username, string Password, string Email, List<RoleType> Roles, Guid Id = default) : ICommand;