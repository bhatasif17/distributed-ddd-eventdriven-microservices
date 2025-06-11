namespace Wakett.Common.Contracts;

public interface IMessageBus
{
    Task PublishAsync<T>(T message, string topicName) where T : IEvent;
}