using Microsoft.EntityFrameworkCore;
using RTN.API.Entities;

namespace RTN.API.Data;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

    public DbSet<NotificationEntity> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(assembly: GetType().Assembly);
    }
}