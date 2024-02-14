using FluentValidation;
using SharedKernel;

namespace Application.Extensions;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule, Error error)
    {
        ArgumentNullException.ThrowIfNull(error, "The error is required");

        return rule.WithErrorCode(error.Code).WithMessage(error.Description);
    }
}