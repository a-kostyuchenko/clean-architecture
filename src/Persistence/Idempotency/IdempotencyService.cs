using Application.Abstractions.Idempotency;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Idempotency;

internal sealed class IdempotencyService(ApplicationDbContext context) : IIdempotencyService
{
    public async Task<bool> RequestExistsAsync(Guid requestId) => 
        await context.Set<IdempotentRequest>().AnyAsync(x => x.Id == requestId);

    public async Task CreateRequestAsync(Guid requestId, string name)
    {
        var idempotentRequest = new IdempotentRequest()
        {
            Id = requestId,
            Name = name,
            CreatedOnUtc = DateTime.UtcNow
        };

        context.Add(idempotentRequest);

        await context.SaveChangesAsync();
    }
}