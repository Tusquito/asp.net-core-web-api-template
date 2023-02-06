using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Backend.Libs.Database.Account;
using Backend.Libs.Domain;
using Backend.Libs.Domain.Commands.Authentication;
using Backend.Libs.Domain.Enums;
using Backend.Libs.Domain.Extensions;
using Backend.Libs.gRPC.Account.Request;
using Backend.Libs.gRPC.Account.Responses;
using Backend.Libs.gRPC.Enums;
using Backend.Libs.Models.Account;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Authentication.Endpoints;

public class RegisterEndpoint : EndpointBaseAsync
    .WithRequest<RegisterRequestForm>
    .WithoutResult
{
    private readonly IValidator<RegisterRequestForm> _validator;
    private readonly ISender _sender;

    public RegisterEndpoint(IValidator<RegisterRequestForm> validator, ISender sender)
    {
        _validator = validator;
        _sender = sender;
    }

    [AllowAnonymous]
    [HttpPost("/register")]
    public override async Task<IActionResult> HandleAsync([FromBody] RegisterRequestForm form,
        CancellationToken cancellationToken = new())
    {
        ValidationResult validationResult = await _validator.ValidateAsync(form, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.ToActionResult();
        }

        RegisterAccountCommand command =
            new RegisterAccountCommand(form.Username, form.Password, form.PasswordConfirmation, form.Email);

        Result result = await _sender.Send(command, cancellationToken);

        return result.ToActionResult();
    }
}