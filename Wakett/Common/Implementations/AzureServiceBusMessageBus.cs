using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Wakett.Common.Contracts;

namespace Wakett.Common.Implementations;

public class AzureServiceBusMessageBus : IMessageBus
{
    private readonly ServiceBusClient _client;
    private ServiceBusSender _sender;
    //private readonly string _topicName = "rate-change-event";

    public AzureServiceBusMessageBus(ServiceBusClient client)
    {
        _client = client;
    }

    public async Task PublishAsync<T>(T message, string topicName) where T : IEvent
    {
        _sender = _client.CreateSender(topicName);
        var json = JsonSerializer.Serialize(message, message.GetType());
        var serviceBusMessage = new ServiceBusMessage(json)
        {
            Subject = typeof(T).FullName
        };

        await _sender.SendMessageAsync(serviceBusMessage);
    }
    
}