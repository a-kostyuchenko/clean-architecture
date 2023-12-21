using Application.Abstractions.Caching;
using Application.Abstractions.Messaging;
using MediatR;

namespace Application.Behaviors;

internal sealed class QueryCachingBehavior<TRequest, TResponse>(ICacheService cacheService) 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        return await cacheService.GetAsync(
            request.Key,
            _ => next(),
            request.Expiration,
            cancellationToken);
    }
}