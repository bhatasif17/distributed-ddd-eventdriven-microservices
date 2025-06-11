using Wakett.Application.Services;

namespace Wakett.BackgroundServices;

public class RateSchedulerSimulator(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Simulate scheduler trigger
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            
            using var scope = serviceProvider.CreateScope();

            var rateService = scope.ServiceProvider.GetRequiredService<RateService>();
            await rateService.FetchRatesAsync();
        }
    }
}