using Application.Abstractions.Cryptography;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Domain.Users.Errors;
using Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using SharedKernel.Result;

namespace Application.Features.Users.Command.Create;

internal sealed class CreateUserHandler(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher) : ICommandHandler<CreateUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        Result<FirstName> firstNameResult = FirstName.Create(request.FirstName);
        Result<LastName> lastNameResult = LastName.Create(request.LastName);
        Result<Email> emailResult = Email.Create(request.Email);
        Result<Password> passwordResult = Password.Create(request.Password);

        var firstFailureOrSuccess = Result.FirstFailureOrSuccess(
            firstNameResult,
            lastNameResult,
            emailResult,
            passwordResult);

        if (firstFailureOrSuccess.IsFailure)
        {
            return Result.Failure<Guid>(firstFailureOrSuccess.Error);
        }

        if (await context.Users.AnyAsync(x => x.Email == emailResult.Value, cancellationToken))
        {
            return Result.Failure<Guid>(UserErrors.EmailAlreadyInUse);
        }

        string passwordHash = passwordHasher.HashPassword(passwordResult.Value);

        var user = User.Create(firstNameResult.Value, lastNameResult.Value, emailResult.Value, passwordHash);
        
        context.Insert(user);

        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Success(user.Id);
    }
}
