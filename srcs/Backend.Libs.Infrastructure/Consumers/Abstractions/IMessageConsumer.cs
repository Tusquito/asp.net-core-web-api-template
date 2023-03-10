using Backend.Libs.Infrastructure.Messages.Abstractions;

namespace Backend.Libs.Infrastructure.Consumers.Abstractions;

public interface IMessageConsumer<in T> where T : IMessage
{
}