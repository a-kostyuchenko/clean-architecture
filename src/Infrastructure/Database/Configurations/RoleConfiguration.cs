using Domain.Roles;
using Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(TableNames.Roles);
        
        builder.HasKey(x => x.Id);

        builder
            .HasMany(x => x.Permissions)
            .WithMany()
            .UsingEntity(joinBuilder =>
            {
                joinBuilder.ToTable("role_permissions");
            });

        builder
            .HasMany(x => x.Users)
            .WithMany(x => x.Roles);

        builder.HasData(Role.GetValues());
    }
}
