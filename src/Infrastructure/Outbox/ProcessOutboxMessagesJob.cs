using Application.Abstractions.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Persistence.Outbox;
using Polly;
using Polly.Retry;
using Quartz;
using SharedKernel;

namespace Infrastructure.Outbox;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob(
    IPublisher publisher,
    IApplicationDbContext dbContext,
    IUnitOfWork unitOfWork, IOptions<OutboxOptions> outboxOptions) : IJob
{
    private readonly OutboxOptions _outboxOptions = outboxOptions.Value;
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    public async Task Execute(IJobExecutionContext context)
    {
        List<OutboxMessage> messages = await dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null &&
                        m.Error == null)
            .Take(_outboxOptions.BatchSize)
            .ToListAsync(context.CancellationToken);

        foreach (OutboxMessage outboxMessage in messages)
        {
            IDomainEvent? domainEvent = JsonConvert
                .DeserializeObject<IDomainEvent>(
                    outboxMessage.Content,
                    JsonSerializerSettings);

            if (domainEvent is null)
            {
                continue;
            }

            AsyncRetryPolicy policy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    _outboxOptions.RetriesCount,
                    attempt => TimeSpan.FromMilliseconds(50 * attempt));

            PolicyResult result = await policy.ExecuteAndCaptureAsync(() =>
                publisher.Publish(
                    domainEvent,
                    context.CancellationToken));

            outboxMessage.Error = result.FinalException?.ToString();
            outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
        }

        await unitOfWork.SaveChangesAsync();
    }
}
