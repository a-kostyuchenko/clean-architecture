using FluentValidation;

namespace Infrastructure.Outbox;

internal sealed class OutboxOptionsValidator : AbstractValidator<OutboxOptions>
{
    public OutboxOptionsValidator()
    {
        RuleFor(x => x.IntervalInSeconds).InclusiveBetween(10, 60);
        
        RuleFor(x => x.RetriesCount).InclusiveBetween(1, 10);
        
        RuleFor(x => x.BatchSize).InclusiveBetween(20, 50);
    }
}