using MediatR;
using Microsoft.EntityFrameworkCore;
using Wakett.Common.Contracts;
using Wakett.Domain.Events;
using Wakett.Infrastructure.Persistence;

namespace Wakett.Domain.Handlers;

public class PositionClosedEventHandler : INotificationHandler<PositionClosedEvent>
{
    private readonly PositionsDbContext _context;
    private readonly IMessageBus _messageBus;

    public PositionClosedEventHandler(
        PositionsDbContext context,
        IMessageBus messageBus)
    {
        _context = context;
        _messageBus = messageBus;
    }

    public async Task Handle(PositionClosedEvent notification, CancellationToken cancellationToken)
    {
        var position = await _context.Positions
            .FirstOrDefaultAsync(p => p.Id == notification.PositionId, cancellationToken);

        if (position == null || !position.IsOpen) return;

        position.Close();
        await _context.SaveChangesAsync(cancellationToken);

        await _messageBus.PublishAsync(new PositionClosedNotification(
            position.Id,
            position.InstrumentId,
            DateTime.UtcNow), "position-closed-event");
    }
}

public record PositionClosedNotification(
    Guid PositionId,
    string InstrumentId,
    DateTime ClosedAt) : IEvent;