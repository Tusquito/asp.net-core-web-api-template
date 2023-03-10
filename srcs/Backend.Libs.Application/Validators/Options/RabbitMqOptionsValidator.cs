using Backend.Libs.Infrastructure.Options;
using FluentValidation;

namespace Backend.Libs.Application.Validators.Options;

public class RabbitMqOptionsValidator : AbstractValidator<RabbitMqOptions>
{
    public RabbitMqOptionsValidator()
    {
        RuleFor(x => x.Hostname)
            .NotEmpty();
        RuleFor(x => x.Port)
            .NotEmpty();
        RuleFor(x => x.Username)
            .NotEmpty();
    }
}