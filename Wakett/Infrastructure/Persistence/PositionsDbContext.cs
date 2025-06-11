using Microsoft.EntityFrameworkCore;
using Wakett.Domain;

namespace Wakett.Infrastructure.Persistence;

public class PositionsDbContext : DbContext
{
    public PositionsDbContext(DbContextOptions<PositionsDbContext> options)
        : base(options) { }
    public DbSet<PositionAggregate> Positions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PositionAggregate>().HasKey(p => p.Id);
    }
}