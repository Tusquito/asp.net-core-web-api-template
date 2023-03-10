using Ardalis.ApiEndpoints;
using Backend.Libs.Application.Commands.Authentication;
using Backend.Libs.Application.Extensions;
using Backend.Libs.Domain;
using Backend.Libs.Domain.Extensions;
using Backend.Libs.Domain.Forms.Authentication;
using Backend.Libs.Domain.Models.Authentication;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Authentication.Endpoints;

public class LoginEndpoint : EndpointBaseAsync
    .WithRequest<LoginRequestForm>
    .WithoutResult
{

    private readonly ISender _sender;
    private readonly IValidator<LoginRequestForm> _validator;
    
    public LoginEndpoint(ISender sender, IValidator<LoginRequestForm> validator)
    {
        _sender = sender;
        _validator = validator;
    }

    [AllowAnonymous]
    [HttpPost("/login")]
    public override async Task<IActionResult> HandleAsync([FromBody] LoginRequestForm form, CancellationToken cancellationToken = new())
    {
        ValidationResult validationResult = await _validator.ValidateAsync(form, cancellationToken);
        
        if (!validationResult.IsValid)
        {
            return validationResult.ToActionResult();
        }

        Result<TokenModel> result = await _sender.Send(new AuthenticateCommand(form.Login, form.Password), cancellationToken);

        return result.ToActionResult();
    }
}