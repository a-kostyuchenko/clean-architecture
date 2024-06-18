using Infrastructure.Constants;
using Infrastructure.Idempotency;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

internal sealed class IdempotentRequestConfiguration : IEntityTypeConfiguration<IdempotentRequest>
{
    public void Configure(EntityTypeBuilder<IdempotentRequest> builder)
    {
        builder.ToTable(TableNames.IdempotentRequests);
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired();
    }
}