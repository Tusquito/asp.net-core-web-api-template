using Backend.Libs.Domain.Enums;
using Backend.Libs.Models.Account;
using FluentValidation;

namespace Backend.Libs.Domain.Validators.Authentication;

public class RegisterRequestValidator : AbstractValidator<RegisterRequestForm>
{

    public const int PasswordMinLength = 4;
    public const int PasswordMaxLength = 16;
    
    public const int UsernameMinLength = 4;
    public const int UsernameMaxLength = 16;
    
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage(ResultMessageKey.BadRequestInvalidUsernameFormat.ToString())
            .Length(UsernameMinLength, UsernameMaxLength)
            .WithMessage(ResultMessageKey.BadRequestInvalidUsernameLength.ToString());
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(ResultMessageKey.BadRequestInvalidPasswordFormat.ToString())
            .Length(PasswordMinLength, PasswordMaxLength)
            .WithMessage(ResultMessageKey.BadRequestInvalidPasswordLength.ToString());
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage(ResultMessageKey.BadRequestInvalidEmailFormat.ToString());
        RuleFor(x => x)
            .Must(x => x.Password == x.PasswordConfirmation)
            .WithMessage(ResultMessageKey.BadRequestDifferentPasswordConfirmation.ToString());

    }
}