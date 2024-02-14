using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;

namespace Persistence.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(TableNames.Users);
        
        builder.HasKey(x => x.Id);

        builder.ComplexProperty(
            u => u.Email,
            b => b.Property(e => e.Value).HasColumnName(nameof(User.Email)).HasMaxLength(Email.MaxLength));
        
        builder.ComplexProperty(
            u => u.FirstName,
            b => b.Property(e => e.Value).HasColumnName(nameof(User.FirstName)).HasMaxLength(FirstName.MaxLength));
        
        builder.ComplexProperty(
            u => u.LastName,
            b => b.Property(e => e.Value).HasColumnName(nameof(User.LastName)).HasMaxLength(LastName.MaxLength));

        builder.HasIndex(x => x.Email).IsUnique();

        builder.Property<string>("_passwordHash")
            .HasField("_passwordHash")
            .HasColumnName("PasswordHash")
            .IsRequired();

        builder.Property(u => u.CreatedOnUtc).IsRequired();

        builder.Property(user => user.Deleted).HasDefaultValue(false);

        builder
            .HasMany(x => x.Roles)
            .WithMany(x => x.Users)
            .UsingEntity<UserRole>();

        builder.HasQueryFilter(user => !user.Deleted);
    }
}