using System.Diagnostics;
using Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Behaviors;

internal sealed class PerformanceBehavior<TRequest, TResponse>(
    ILogger<PerformanceBehavior<TRequest, TResponse>> logger,
    Stopwatch timer)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private const int MaxEllapsedMilliseconds = 1000;
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        timer.Start();

        var result = await next();
        
        timer.Stop();
        
        var elapsedMilliseconds = timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > MaxEllapsedMilliseconds)
        {
            logger.LogWarning(
                "Long running request {@RequestName}, {@ElapsedMilliseconds} ms, {@DateTimeUtc}",
                typeof(TRequest).Name,
                elapsedMilliseconds,
                DateTime.UtcNow);
        }

        return result;
    }
}