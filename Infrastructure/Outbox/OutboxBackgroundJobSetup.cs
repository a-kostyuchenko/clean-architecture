using Microsoft.Extensions.Options;
using Quartz;

namespace Infrastructure.Outbox;

public class OutboxBackgroundJobSetup(IOptions<OutboxOptions> outboxOptions) 
    : IConfigureOptions<QuartzOptions>
{
    private readonly OutboxOptions _outboxOptions = outboxOptions.Value;

    public void Configure(QuartzOptions options)
    {
        var jobKey = JobKey.Create(nameof(ProcessOutboxMessagesJob));

        options.AddJob<ProcessOutboxMessagesJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
            .AddTrigger(trigger =>
                trigger
                    .ForJob(jobKey)
                    .WithSimpleSchedule(schedule =>
                        schedule
                            .WithIntervalInSeconds(_outboxOptions.IntervalInSeconds)
                            .RepeatForever()));
    }
}