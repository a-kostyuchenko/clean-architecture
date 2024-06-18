using Domain.Users;
using Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(TableNames.Users);
        
        builder.HasKey(x => x.Id);

        builder.ComplexProperty(
            u => u.Email,
            b => b.Property(e => e.Value).HasMaxLength(Email.MaxLength));
        
        builder.ComplexProperty(
            u => u.FirstName,
            b => b.Property(e => e.Value).HasMaxLength(FirstName.MaxLength));
        
        builder.ComplexProperty(
            u => u.LastName,
            b => b.Property(e => e.Value).HasMaxLength(LastName.MaxLength));
        
        builder.Property<string>("_passwordHash")
            .HasField("_passwordHash")
            .HasColumnName("PasswordHash")
            .IsRequired();

        builder.Property(u => u.CreatedOnUtc).IsRequired();

        builder.Property(user => user.Deleted).HasDefaultValue(false);

        builder
            .HasMany(x => x.Roles)
            .WithMany(x => x.Users)
            .UsingEntity(joinBuilder =>
            {
                joinBuilder.ToTable("user_roles");
            });

        builder.HasQueryFilter(user => !user.Deleted);
    }
}
