using MediatR;

namespace Wakett.Domain.Events;

public class RateChangedEvent : INotification
{
    public string Symbol { get; }
    public decimal NewRate { get; }
    public decimal OldRate { get; }

    public RateChangedEvent(string symbol, decimal newRate, decimal oldRate)
        => (Symbol, NewRate, OldRate) = (symbol, newRate, oldRate);
}