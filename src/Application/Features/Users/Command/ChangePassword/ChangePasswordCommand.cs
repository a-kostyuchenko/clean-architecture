using Application.Abstractions.Messaging;

namespace Application.Features.Users.Command.ChangePassword;

public sealed record ChangePasswordCommand(Guid UserId, string Password) : ICommand;
