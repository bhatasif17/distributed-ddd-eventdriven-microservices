using MediatR;
using Wakett.Application.Services;
using Wakett.Domain.Events;

namespace Wakett.Domain.Handlers;

public class RateChangedEventHandler : INotificationHandler<RateChangedEvent>
{
    private readonly PositionService _positionService;

    public RateChangedEventHandler(PositionService positionService)
        => _positionService = positionService;

    public async Task Handle(RateChangedEvent notification, CancellationToken token)
        => await _positionService.HandleRateChange(
            notification.Symbol, 
            notification.NewRate);
}