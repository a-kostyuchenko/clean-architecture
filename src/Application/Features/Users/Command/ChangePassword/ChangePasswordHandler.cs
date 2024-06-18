using Application.Abstractions;
using Application.Abstractions.Cryptography;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.Users.Command.ChangePassword;

internal sealed class ChangePasswordHandler(
        IApplicationDbContext context,
        IUserContext userContext,
        IPasswordHasher passwordHasher)
    : ICommandHandler<ChangePasswordCommand>
{
    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId != userContext.UserId)
        {
            return Result.Failure(UserErrors.InvalidPermissions);
        }

        Result<Password> passwordResult = Password.Create(request.Password);

        if (passwordResult.IsFailure)
        {
            return passwordResult;
        }

        User? user = await context.Users
            .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
        
        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        string passwordHash = passwordHasher.HashPassword(passwordResult.Value);

        Result result = user.ChangePassword(passwordHash);

        if (result.IsFailure)
        {
            return result;
        }

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
