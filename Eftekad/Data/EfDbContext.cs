using System.Linq.Expressions;
using Eftekad.Features.AcademicStages;
using Eftekad.Features.Members;
using Eftekad.Features.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Eftekad.Data;

public class EfDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<AcademicStage> AcademicStages { get; set; }
    public DbSet<Member> Members { get; set; }

    public override EntityEntry<TEntity> Remove<TEntity>(TEntity entity) where TEntity : class
    {
        if (entity is BaseEntity baseEntity)
        {
            // Instead of removing, mark as deleted
            baseEntity.IsDeleted = true;
            baseEntity.DeletedAt = DateTime.UtcNow;
            Entry(entity).State = EntityState.Modified;
            return Entry(entity);
        }
        
        return base.Remove(entity);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EfDbContext).Assembly);

        
        // Apply global query filter for all entities that inherit from BaseEntity
        foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                     .Where(e => typeof(BaseEntity).IsAssignableFrom(e.ClrType)))
        {
            // Add filter to exclude soft-deleted entities
            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var property = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
            var condition = Expression.Equal(property, Expression.Constant(false));
            var lambda = Expression.Lambda(condition, parameter);

            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
        }
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseEntity);

        var now = DateTime.UtcNow;
        foreach (var entry in entries)
        {
            var entity = (BaseEntity)entry.Entity;
            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = now;
            }
            entity.UpdatedAt = now;
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}