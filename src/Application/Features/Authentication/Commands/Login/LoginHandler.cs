using Application.Abstractions;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Domain.Users.Errors;
using Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Result;

namespace Application.Features.Authentication.Commands.Login;

internal sealed class LoginHandler(
    IApplicationDbContext context,
    IPasswordHashChecker passwordHashChecker, 
    IJwtProvider jwtProvider) : ICommandHandler<LoginCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        Result<Email> emailResult = Email.Create(request.Email);

        if (emailResult.IsFailure)
        {
            return Result.Failure<LoginResponse>(AuthenticationErrors.InvalidEmail);
        }

        User? user = await context.Users.FirstOrDefaultAsync(
            u => u.Email == emailResult.Value,
            cancellationToken);

        if (user is null)
        {
            return Result.Failure<LoginResponse>(AuthenticationErrors.UserNotFound);
        }

        bool passwordValid = user.VerifyPasswordHash(request.Password, passwordHashChecker);

        if (!passwordValid)
        {
            return Result.Failure<LoginResponse>(AuthenticationErrors.InvalidPassword);
        }

        string token = await jwtProvider.GenerateAsync(user);

        return Result.Success(new LoginResponse(token));
    }
}
