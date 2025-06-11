using MediatR;

namespace Wakett.Domain.Events;

public class PositionValueUpdatedEvent : INotification
{
    public Guid PositionId { get; }
    public decimal NewValue { get; }

    public PositionValueUpdatedEvent(Guid positionId, decimal newValue)
        => (PositionId, NewValue) = (positionId, newValue);
}