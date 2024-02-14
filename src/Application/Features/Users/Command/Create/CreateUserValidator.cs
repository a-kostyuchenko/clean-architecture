using FluentValidation;

namespace Application.Features.Users.Command.Create;

internal sealed class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(u => u.Email).NotEmpty();

        RuleFor(u => u.Password).NotEmpty();

        RuleFor(u => u.FirstName).NotEmpty();
        
        RuleFor(u => u.LastName).NotEmpty();
    }
}