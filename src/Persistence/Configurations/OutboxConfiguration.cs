using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;
using Persistence.Outbox;

namespace Persistence.Configurations;

internal sealed class OutboxConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable(TableNames.OutboxMessages);
        
        builder.HasKey(x => x.Id);
        
        builder.Property(o => o.Content).HasColumnType("jsonb");
    }
}
