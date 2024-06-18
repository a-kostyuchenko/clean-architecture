using System.Reflection;
using Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Permission = Domain.Roles.Permission;

namespace Infrastructure.Database.Configurations;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(TableNames.Permissions);
        
        builder.HasKey(x => x.Id);

        IEnumerable<Permission> permissions = Domain.AssemblyReference.Assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsClass: true } &&
                           type.IsSubclassOf(typeof(SharedKernel.Permission)))
            .SelectMany(type => type
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fieldInfo => typeof(SharedKernel.Permission).IsAssignableFrom(fieldInfo.FieldType))
                    .Select(fieldInfo => (SharedKernel.Permission)fieldInfo.GetValue(default)!)
        .Select(permission => Permission.Create(
            permission.Key,
            permission.Name,
            permission.Description))
        .ToList());
        
        builder.HasData(permissions);
    }
}
