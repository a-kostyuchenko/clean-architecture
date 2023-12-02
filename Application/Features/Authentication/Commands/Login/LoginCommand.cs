using Application.Abstractions.Messaging;

namespace Application.Features.Authentication.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand<TokenResponse>;

public record TokenResponse(string Token);