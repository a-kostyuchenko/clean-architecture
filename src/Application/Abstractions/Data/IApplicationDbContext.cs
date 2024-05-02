using Domain.Roles;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Permission = Domain.Roles.Permission;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
    #region DbSets

    DbSet<User> Users { get; set; }
    DbSet<Role> Roles { get; set; }
    DbSet<Permission> Permissions { get; set; }

    #endregion

    #region Methods
    DbSet<TEntity> Set<TEntity>() 
        where TEntity : class;
    Task<TEntity?> GetBydIdAsync<TEntity>(Guid id)
        where TEntity : Entity;
    void Insert<TEntity>(TEntity entity)
        where TEntity : Entity;
    void InsertRange<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : Entity;
    void Remove<TEntity>(TEntity entity) 
        where TEntity : Entity;
    void RemoveRange<TEntity>(IEnumerable<TEntity> entity)
        where TEntity : Entity;

    #endregion
}