namespace Application.Features.Users.Command.Create;

public record CreateUserRequest(
    string LastName,
    string FirstName,
    string Email,
    string Password);