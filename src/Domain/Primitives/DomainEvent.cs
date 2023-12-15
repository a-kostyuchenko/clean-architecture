using MediatR;

namespace Domain.Primitives;

public abstract record DomainEvent(Guid Id) : INotification;