using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SharedKernel;

namespace Infrastructure.Configuration;

public class FluentValidateOptions<TOptions>(IServiceProvider serviceProvider, string? name)
    : IValidateOptions<TOptions>
    where TOptions : class
{
    public ValidateOptionsResult Validate(string? name1, TOptions options)
    {
        if (!string.IsNullOrWhiteSpace(name) && name != name1)
            return ValidateOptionsResult.Skip;
        
        Ensure.NotNull(options);

        using var scope = serviceProvider.CreateScope();

        var validator = scope.ServiceProvider.GetRequiredService<IValidator<TOptions>>();

        var result = validator.Validate(options);

        if (result.IsValid)
            return ValidateOptionsResult.Success;

        var type = options.GetType().Name;
        var errors = result.Errors.Select(failure =>
            $"Validation failed for {type}.{failure.PropertyName} with the error {failure.ErrorMessage}").ToList();

        return ValidateOptionsResult.Fail(errors);
    }
}