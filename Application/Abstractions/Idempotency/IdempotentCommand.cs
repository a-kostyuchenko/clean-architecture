using Application.Abstractions.Messaging;

namespace Application.Abstractions.Idempotency;

public abstract record IdempotentCommand(Guid RequestId) : ICommand;