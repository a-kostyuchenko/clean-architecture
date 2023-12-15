using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;
using Persistence.Idempotency;

namespace Persistence.Configurations;

internal sealed class IdempotentRequestConfiguration : IEntityTypeConfiguration<IdempotentRequest>
{
    public void Configure(EntityTypeBuilder<IdempotentRequest> builder)
    {
        builder.ToTable(TableNames.IdempotentRequests);
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired();
    }
}