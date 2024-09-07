using Microsoft.EntityFrameworkCore;

using RTN.API.Data.Entities;

namespace RTN.API.Data;

public class MyDbContext(DbContextOptions<MyDbContext> options) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder builder) {
        builder.Ignore<BaseEntity>();
        builder.ApplyConfigurationsFromAssembly(assembly: GetType().Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity
                     && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries) {
            if (entry.State == EntityState.Added) {
                ((BaseEntity)entry.Entity).CreatedAt = DateTime.UtcNow;
            }

            ((BaseEntity)entry.Entity).UpdatedAt = DateTime.UtcNow;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
