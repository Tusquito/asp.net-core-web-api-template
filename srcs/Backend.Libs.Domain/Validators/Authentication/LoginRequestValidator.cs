using Backend.Libs.Domain.Enums;
using Backend.Libs.Models.Authentication;
using FluentValidation;

namespace Backend.Libs.Domain.Validators.Authentication;

public class LoginRequestValidator : AbstractValidator<LoginRequestForm>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty()
            .WithErrorCode(ResultMessageKey.BadRequestLoginInputMissingOnLogin.ToString());
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithErrorCode(ResultMessageKey.BadRequestPasswordInputMissingOnLogin.ToString());
    }
}