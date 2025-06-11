using Wakett.Infrastructure.Persistence;

namespace Wakett.Domain;

public class RateAggregate : Entity
{
    public string Symbol { get; private set; }
    private readonly List<RateHistory> _history = new();
    public IReadOnlyCollection<RateHistory> History => _history.AsReadOnly();

    public RateAggregate(string symbol) => Symbol = symbol;

    public void AddHistoryEntry(decimal rate)
    {
        _history.Add(new RateHistory(rate));
        CleanOldHistory();
    }

    public bool CheckSignificantChange(decimal currentRate, out decimal oldRate)
    {
        oldRate = _history.OrderBy(h => h.Timestamp)
            .FirstOrDefault()?.Rate ?? 0;
        
        return oldRate > 0 && 
               Math.Abs(currentRate - oldRate) / oldRate > 0.05m;
        
        //return true;
    }

    private void CleanOldHistory() 
        => _history.RemoveAll(h => h.Timestamp < DateTime.UtcNow.AddHours(-24));
}