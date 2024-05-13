using Application.Abstractions.Idempotency;
using MediatR;

namespace Application.Behaviors;

internal sealed class IdempotentBehavior<TRequest, TResponse>(IIdempotencyService idempotencyService)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IdempotentCommand
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (await idempotencyService.RequestExistsAsync(request.RequestId))
        {
            return default!;
        }

        await idempotencyService.CreateRequestAsync(request.RequestId, typeof(TRequest).Name);

        TResponse response = await next();

        return response;
    }
}
