using Application.Abstractions.Messaging;

namespace Application.Features.Users.Command.ChangePassword;

public record ChangePasswordCommand(Guid UserId, string Password) : ICommand;