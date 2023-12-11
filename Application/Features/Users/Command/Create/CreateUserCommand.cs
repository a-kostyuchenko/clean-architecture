using Application.Abstractions.Messaging;

namespace Application.Features.Users.Command.Create;

public record CreateUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : ICommand<Guid>;