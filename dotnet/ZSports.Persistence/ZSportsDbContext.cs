using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZSports.Domain.Entities;

namespace ZSports.Persistence;

public class ZSportsDbContext(DbContextOptions opts): IdentityDbContext<Usuario, IdentityRole<Guid>, Guid>(opts)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("zsports");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ZSportsDbContext).Assembly);
    }
}
