using System.Text.Json;
using Azure.Messaging.ServiceBus;
using MediatR;
using Wakett.Common.Contracts;
using Wakett.Domain.Handlers;

namespace Wakett.BackgroundServices;

public class PositionEventProcessor : BackgroundService
{
    private readonly ServiceBusProcessor _processor;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PositionEventProcessor> _logger;
    private readonly IMessageBus _messageBus;

    public PositionEventProcessor(
        ServiceBusClient client,
        IServiceProvider serviceProvider,
        ILogger<PositionEventProcessor> logger,
        IConfiguration configuration, IMessageBus messageBus)
    {
        var topicName = configuration["AzureServiceBus:TopicName"] ?? "rate-change-event";
        var subscriptionName = configuration["AzureServiceBus:SubscriptionName"] ?? "position-subscriber";

        _processor = client.CreateProcessor(
            topicName,
            subscriptionName, 
            new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = false,
                MaxConcurrentCalls = 5
            });

        _serviceProvider = serviceProvider;
        _logger = logger;
        _messageBus = messageBus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _processor.ProcessMessageAsync += ProcessMessageHandler;
        _processor.ProcessErrorAsync += ErrorHandler;

        _logger.LogInformation("PositionEventProcessor handlers attached. Starting processor...");
        await _processor.StartProcessingAsync(stoppingToken);
        _logger.LogInformation("Started processing messages for {Topic}", _processor.EntityPath);
    }

    private async Task ProcessMessageHandler(ProcessMessageEventArgs args)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var messageType = args.Message.Subject;
            var body = args.Message.Body.ToString();

            switch (messageType)
            {
                case nameof(PositionValueDto):
                    var rateEvent = JsonSerializer.Deserialize<PositionValueDto>(body);
                    if (rateEvent != null)
                    {
                        //i haven't created update-rate-event in azure
                        await _messageBus.PublishAsync(rateEvent with {UpdatedAt = DateTime.UtcNow}, "update-rate-event");
                        //await mediator.Publish(rateEvent, args.CancellationToken);
                    }
                    break;
            }

            await args.CompleteMessageAsync(args.Message, args.CancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message");
            await args.AbandonMessageAsync(args.Message, cancellationToken: args.CancellationToken);
        }
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception, 
            "Service Bus Error: {ErrorSource}", args.ErrorSource);
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping Position Event Processor");
        await _processor.CloseAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}