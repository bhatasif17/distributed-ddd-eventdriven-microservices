using MediatR;
using Wakett.Common.Contracts;
using Wakett.Domain.Events;

namespace Wakett.Domain.Handlers;

public class PositionValueUpdatedHandler : INotificationHandler<PositionValueUpdatedEvent>
{
    private readonly IMessageBus _messageBus;
    private readonly ILogger<PositionValueUpdatedHandler> _logger;

    public PositionValueUpdatedHandler(
        IMessageBus messageBus,
        ILogger<PositionValueUpdatedHandler> logger)
    {
        _messageBus = messageBus;
        _logger = logger;
    }

    public async Task Handle(PositionValueUpdatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            await _messageBus.PublishAsync(new PositionValueDto(
                notification.PositionId,
                notification.NewValue,
                DateTime.UtcNow), "rate-change-event"); //rate-change-event exists in Azure

            _logger.LogInformation("Published position {PositionId} update: {Value}",
                notification.PositionId,
                notification.NewValue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish position value update");
            throw; // Retry policy will handle this
        }
    }
}

// DTO for queue message
public record PositionValueDto(
    Guid PositionId,
    decimal CurrentValue,
    DateTime UpdatedAt) : IEvent;