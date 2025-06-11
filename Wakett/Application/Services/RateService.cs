using MediatR;
using Microsoft.EntityFrameworkCore;
using Wakett.Domain;
using Wakett.Domain.Events;
using Wakett.Infrastructure.Clients;
using Wakett.Infrastructure.Persistence;

namespace Wakett.Application.Services;

public class RateService
{
    private readonly RatesDbContext _context;
    private readonly IMediator _mediator;
    private readonly CoinMarketCapClient _client;

    public RateService(
        RatesDbContext context, 
        IMediator mediator,
        CoinMarketCapClient client)
    {
        _context = context;
        _mediator = mediator;
        _client = client;
    }

    public async Task FetchRatesAsync()
    {
        var cryptos = await _client.GetLatestRatesAsync();
        
        foreach (var crypto in cryptos)
        {
            var rate = await _context.Rates
                .Include(r => r.History)
                .FirstOrDefaultAsync(r => r.Symbol == crypto.Symbol);

            if (rate == null)
            {
                rate = new RateAggregate(crypto.Symbol);
                rate.AddHistoryEntry(crypto.Price);
                _context.Rates.Add(rate);
            }
            else
            {
                if (rate.CheckSignificantChange(crypto.Price, out decimal oldRate))
                {
                    await _mediator.Publish(new RateChangedEvent(
                        crypto.Symbol, 
                        crypto.Price, 
                        oldRate
                    ));
                }

                rate.AddHistoryEntry(crypto.Price);
                _context.Rates.Update(rate);
            }
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}