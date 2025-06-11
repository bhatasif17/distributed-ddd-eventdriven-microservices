using Wakett.Infrastructure.Persistence;

namespace Wakett.Domain;

public class PositionAggregate : Entity
{
    public string InstrumentId { get; set; }
    public decimal Quantity { get; set; }
    public decimal InitialRate { get; set; }
    public int Side { get; set; } // +1 = BUY, -1 = SELL
    public bool IsOpen { get; set; } = true;

    public PositionAggregate(
        Guid id, 
        string instrumentId, 
        decimal quantity, 
        decimal initialRate, 
        int side)
    {
        Id = id;
        InstrumentId = instrumentId;
        Quantity = quantity;
        InitialRate = initialRate;
        Side = side;
    }

    public void Close() => IsOpen = false;

    public decimal CalculateProfitLoss(decimal currentRate) 
        => Quantity * (currentRate - InitialRate) * Side;
}