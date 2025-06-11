using MediatR;

namespace Wakett.Domain.Events;

public class PositionClosedEvent : INotification
{
    public Guid PositionId { get; }

    public PositionClosedEvent(Guid positionId) => (PositionId) = (positionId);
}