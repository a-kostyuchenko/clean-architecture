using Application.Abstractions;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Services;
using Domain.Shared;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Authentication.Commands.Login;

internal sealed class LoginHandler(
    IApplicationDbContext context,
    IPasswordHashChecker passwordHashChecker, 
    IJwtProvider jwtProvider) : ICommandHandler<LoginCommand, TokenResponse>
{
    public async Task<Result<TokenResponse>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        Result<Email> emailResult = Email.Create(request.Email);

        if (emailResult.IsFailure)
            return Result.Failure<TokenResponse>(DomainErrors.Authentication.InvalidEmail);

        var user = await context.Users.FirstOrDefaultAsync(
            u => u.Email == emailResult.Value,
            cancellationToken);

        if (user is null)
            return Result.Failure<TokenResponse>(DomainErrors.Authentication.UserNotFound);

        bool passwordValid = user.VerifyPasswordHash(request.Password, passwordHashChecker);

        if (!passwordValid)
            return Result.Failure<TokenResponse>(DomainErrors.Authentication.InvalidPassword);

        string token = await jwtProvider.GenerateAsync(user);

        return Result.Success(new TokenResponse(token));
    }
}