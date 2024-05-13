using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SharedKernel;

namespace Infrastructure.Configuration;

public class FluentValidateOptions<TOptions>(IServiceProvider serviceProvider, string? optionsName)
    : IValidateOptions<TOptions>
    where TOptions : class
{
    public ValidateOptionsResult Validate(string? name, TOptions options)
    {
        if (!string.IsNullOrWhiteSpace(optionsName) && optionsName != name)
        {
            return ValidateOptionsResult.Skip;
        }

        Ensure.NotNull(options);

        using IServiceScope scope = serviceProvider.CreateScope();

        IValidator<TOptions> validator = scope.ServiceProvider.GetRequiredService<IValidator<TOptions>>();

        ValidationResult? result = validator.Validate(options);

        if (result.IsValid)
        {
            return ValidateOptionsResult.Success;
        }

        string type = options.GetType().Name;
        var errors = result.Errors.Select(failure =>
            $"Validation failed for {type}.{failure.PropertyName} with the error {failure.ErrorMessage}").ToList();

        return ValidateOptionsResult.Fail(errors);
    }
}
