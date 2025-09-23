using Microsoft.EntityFrameworkCore;

namespace ZSports.Persistence;

public class ZSportsDbContext(DbContextOptions opts): DbContext(opts)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("zsports");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ZSportsDbContext).Assembly);
    }
}
