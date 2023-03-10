using Backend.Libs.Infrastructure.Messages.Abstractions;

namespace Backend.Libs.Infrastructure.Producers.Abstractions;

public interface IMessageProducer
{
    Task ProduceAsync<T>(T message, CancellationToken cancellationToken = default) where T : IMessage;
    Task ProduceAsync<T>(List<T> messages, CancellationToken cancellationToken = default) where T : IMessage;
}