using Application.Abstractions;
using Application.Abstractions.Cryptography;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Command.ChangePassword;

internal sealed class ChangePasswordHandler(
        IApplicationDbContext context,
        IUserIdentifierProvider userIdentifierProvider,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher)
    : ICommandHandler<ChangePasswordCommand>
{
    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId != userIdentifierProvider.UserId)
            return Result.Failure(DomainErrors.User.InvalidPermissions);

        Result<Password> passwordResult = Password.Create(request.Password);

        if (passwordResult.IsFailure)
            return Result.Failure(passwordResult.Error);

        User? user = await context.Users
            .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
        
        if (user is null)
            return Result.Failure(DomainErrors.User.NotFound);

        string passwordHash = passwordHasher.HashPassword(passwordResult.Value);

        Result result = user.ChangePassword(passwordHash);

        if (result.IsFailure)
            return Result.Failure(result.Error);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}