using Backend.Libs.Infrastructure.Options;
using FluentValidation;

namespace Backend.Libs.Application.Validators.Options;

public class RedisOptionsValidator : AbstractValidator<RedisOptions>
{
    public RedisOptionsValidator()
    {
        RuleFor(x => x.Port)
            .NotEmpty();
        RuleFor(x => x.Host)
            .NotEmpty();
    }
}