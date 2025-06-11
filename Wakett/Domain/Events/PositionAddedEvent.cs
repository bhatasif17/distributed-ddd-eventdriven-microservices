using MediatR;

namespace Wakett.Domain.Events;

public class PositionAddedEvent: INotification
{
    public string InstrumentId { get; }
    public decimal Quantity { get; }
    public decimal InitialRate { get; }
    public int Side { get; }

    public PositionAddedEvent(string instrumentId, decimal quantity, decimal initialRate, int side)
    {
        InstrumentId = instrumentId;
        Quantity = quantity;
        InitialRate = initialRate;
        Side = side;
    }
}