using Application.Abstractions.Caching;

namespace Application.Features.Users.Queries.GetById;

public sealed record GetUserByIdQuery(Guid UserId) : ICachedQuery<UserResponse>
{
    public string CacheKey => $"user-by-id-{UserId}";
    public TimeSpan? Expiration => null;
}
