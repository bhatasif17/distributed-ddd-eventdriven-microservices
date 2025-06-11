using Wakett.Infrastructure.Persistence;

namespace Wakett.Domain;

public class RateHistory : Entity
{
    public decimal Rate { get; private set; }
    public DateTime Timestamp { get; private set; } = DateTime.UtcNow;

    public RateHistory(decimal rate)
    {
        Rate = rate;
    }
}