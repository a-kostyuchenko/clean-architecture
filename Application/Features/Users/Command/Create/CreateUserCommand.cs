using Application.Abstractions.Messaging;
using Application.Features.Authentication.Commands.Login;

namespace Application.Features.Users.Command.Create;

public record CreateUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : ICommand<Guid>;