using Application.Abstractions;
using Application.Abstractions.Cryptography;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Features.Authentication.Commands.Login;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Command.Create;

internal sealed class CreateUserHandler(
    IApplicationDbContext context,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher) : ICommandHandler<CreateUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var firstNameResult = FirstName.Create(request.FirstName);
        var lastNameResult = LastName.Create(request.LastName);
        var emailResult = Email.Create(request.Email);
        var passwordResult = Password.Create(request.Password);

        Result firstFailureOrSuccess = Result.FirstFailureOrSuccess(
            firstNameResult,
            lastNameResult,
            emailResult,
            passwordResult);

        if (firstFailureOrSuccess.IsFailure)
            return Result.Failure<Guid>(firstFailureOrSuccess.Error);

        if (await context.Users.AnyAsync(x => x.Email == emailResult.Value, cancellationToken))
            return Result.Failure<Guid>(DomainErrors.User.EmailAlreadyInUse);
        
        string passwordHash = passwordHasher.HashPassword(passwordResult.Value);

        var user = User.Create(firstNameResult.Value, lastNameResult.Value, emailResult.Value, passwordHash);

        context.Insert(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(user.Id);
    }
}