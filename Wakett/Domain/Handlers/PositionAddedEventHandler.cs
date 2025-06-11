using MediatR;
using Wakett.Domain.Events;
using Wakett.Infrastructure.Persistence;

namespace Wakett.Domain.Handlers;

public class PositionAddedHandler : INotificationHandler<PositionAddedEvent>
{
    private readonly PositionsDbContext _context;

    public PositionAddedHandler(PositionsDbContext context)
    {
        _context = context;
    }

    public async Task Handle(PositionAddedEvent notification, CancellationToken cancellationToken)
    {
        var position = new PositionAggregate(
            Guid.NewGuid(),
            notification.InstrumentId,
            notification.Quantity,
            notification.InitialRate,
            notification.Side);

        _context.Positions.Add(position);
        await _context.SaveChangesAsync(cancellationToken);
    }
}