namespace Application.Abstractions;

public interface IUserIdentifierProvider
{
    Guid UserId { get; }
}