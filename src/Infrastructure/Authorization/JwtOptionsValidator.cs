using FluentValidation;

namespace Infrastructure.Authorization;

internal sealed class JwtOptionsValidator : AbstractValidator<JwtOptions>
{
    public JwtOptionsValidator()
    {
        RuleFor(x => x.SecretKey).NotEmpty();

        RuleFor(x => x.Audience).NotEmpty();

        RuleFor(x => x.Issuer).NotEmpty();

        RuleFor(x => x.LifeTime).GreaterThan(0);
    }
}