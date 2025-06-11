using Microsoft.EntityFrameworkCore;
using Wakett.Domain;

namespace Wakett.Infrastructure.Persistence;

public class RatesDbContext : DbContext
{
    public RatesDbContext(DbContextOptions<RatesDbContext> options)
        : base(options) { }

    public DbSet<RateAggregate> Rates { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RateAggregate>().HasKey(r => r.Symbol);
        modelBuilder.Entity<RateAggregate>().OwnsMany(r => r.History);
    }
}