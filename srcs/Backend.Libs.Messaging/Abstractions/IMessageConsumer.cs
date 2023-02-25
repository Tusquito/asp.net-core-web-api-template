using Backend.Libs.Mediator.Messaging.Abstractions;

namespace Backend.Libs.Messaging.Abstractions;

public interface IMessageConsumer<in T> where T : IMessage
{
}