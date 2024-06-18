using Application.Abstractions.Messaging;

namespace Application.Features.Users.Command.Create;

public sealed record CreateUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : ICommand<Guid>;
