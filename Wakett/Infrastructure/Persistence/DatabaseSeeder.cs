using Microsoft.EntityFrameworkCore;
using Wakett.Domain;

namespace Wakett.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(PositionsDbContext context)
    {
        if (await context.Positions.AnyAsync()) return;

        var positions = new List<PositionAggregate>
        {
            new PositionAggregate (Guid.NewGuid(), "BTC/USD", 3, 58871.01215m, 1 ),
            new PositionAggregate (Guid.NewGuid(), "ETH/USD", 10, 2682.019189m, -1 ),
            new PositionAggregate (Guid.NewGuid(), "SOL/USD", 20, 138.5050875m, 1 ),
            new PositionAggregate (Guid.NewGuid(), "BNB/USD", 5, 512.9499832m, 1 ),
            new PositionAggregate (Guid.NewGuid(), "USDT/USD", 10000, 1.000134593m, 1 ),
            new PositionAggregate (Guid.NewGuid(), "ADA/USD", 5000, 0.335245269m, -1 ),
            new PositionAggregate (Guid.NewGuid(), "SHIB/USD", 100000, 0.0000136410407145078m, 1 ),
            new PositionAggregate (Guid.NewGuid(), "DOGE/USD", 43000, 0.105241227m, 1 ),
            new PositionAggregate (Guid.NewGuid(), "XRP/USD", 27000, 0.565457483m, -1 ),
            new PositionAggregate (Guid.NewGuid(), "AVAX/USD", 50, 21.02913658m, -1 ),
            new PositionAggregate (Guid.NewGuid(), "LTC/USD", 10, 61.03340933m, 1 ),
            new PositionAggregate (Guid.NewGuid(), "CRO/USD", 80000, 0.087408805m, 1 ),
            new PositionAggregate (Guid.NewGuid(), "XLM/USD", 63000, 0.09764245m, -1 ),
        };

        context.Positions.AddRange(positions);
        await context.SaveChangesAsync();
    }
}