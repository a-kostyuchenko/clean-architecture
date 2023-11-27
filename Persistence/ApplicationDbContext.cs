using Application.Abstractions.Data;
using Domain.Entities;
using Domain.Primitives;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public sealed class ApplicationDbContext(DbContextOptions options) 
    : DbContext(options), IApplicationDbContext, IUnitOfWork
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => 
        await base.SaveChangesAsync(cancellationToken);
    
    public new DbSet<TEntity> Set<TEntity>()
        where TEntity : Entity
        => base.Set<TEntity>();

    public async Task<TEntity?> GetBydIdAsync<TEntity>(Guid id)
        where TEntity : Entity
        => await Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id);
    
    public void Insert<TEntity>(TEntity entity)
        where TEntity : Entity
        => Set<TEntity>().Add(entity);
    
    public void InsertRange<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : Entity
        => Set<TEntity>().AddRange(entities);

    public new void Remove<TEntity>(TEntity entity)
        where TEntity : Entity
        => Set<TEntity>().Remove(entity);
    
    public void RemoveRange<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : Entity
        => Set<TEntity>().RemoveRange(entities);
}