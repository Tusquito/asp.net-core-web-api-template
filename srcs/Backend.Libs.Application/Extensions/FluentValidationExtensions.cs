using Backend.Libs.Domain;
using Backend.Libs.Domain.Enums;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Libs.Application.Extensions;

public static class FluentValidationExtensions
{
    public static IActionResult ToActionResult(this ValidationResult result)
    {
        ResultMessageKey[] messageKeys = result.Errors.Select(x => Enum.Parse<ResultMessageKey>(x.ErrorCode)).ToArray();
        return result.IsValid switch
        {
            true => DomainResults.Ok(),
            false => DomainResults.BadRequest(messageKeys)
        };
    }
}