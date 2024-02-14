using Application.Abstractions.Messaging;

namespace Application.Features.Users.Queries.GetById;

public record GetUserByIdQuery(Guid UserId) : ICachedQuery<UserResponse>
{
    public string CacheKey => $"user-by-id-{UserId}";
    public TimeSpan? Expiration => null;
}