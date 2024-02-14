using FluentValidation;

namespace Application.Features.Users.Command.ChangePassword;

internal sealed class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.Password).NotEmpty();
    }
}