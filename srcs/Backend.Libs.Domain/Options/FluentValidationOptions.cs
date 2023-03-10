using FluentValidation;
using Microsoft.Extensions.Options;

namespace Backend.Libs.Domain.Options;

public class FluentValidationOptions<TOptions> : IValidateOptions<TOptions> where TOptions : class
{
    private readonly string _name;
    private readonly IValidator<TOptions> _validator;
    public FluentValidationOptions(string name, IValidator<TOptions> validator)
    {
        _name = name;
        _validator = validator;
    }

    public ValidateOptionsResult Validate(string? name, TOptions options)
    {
        if(!string.IsNullOrEmpty(_name) && _name != name)
        {
            return ValidateOptionsResult.Skip;
        }
        
        ArgumentNullException.ThrowIfNull(options);
        
        var result = _validator.Validate(options);
        
        if(!result.IsValid)
        {
            return ValidateOptionsResult.Fail(result.Errors.Select(e => e.ErrorMessage).ToList());
        }
        
        return ValidateOptionsResult.Success;
    }
}