using MediatR;
using Microsoft.EntityFrameworkCore;
using Wakett.Domain.Events;
using Wakett.Infrastructure.Persistence;

namespace Wakett.Application.Services;

public class PositionService
{
    private readonly PositionsDbContext _context;
    private readonly IMediator _mediator;

    public PositionService(
        PositionsDbContext context, 
        IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task HandleRateChange(string symbol, decimal newRate)
    {
        var instrumentId = $"{symbol}/USD";
        var positions = await _context.Positions
            .Where(p => p.InstrumentId == instrumentId && p.IsOpen)
            .ToListAsync();

        foreach (var position in positions)
        {
            var newValue = position.CalculateProfitLoss(newRate);
            await _mediator.Publish(
                new PositionValueUpdatedEvent(position.Id, newValue));
        }
    }
    
}