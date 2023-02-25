using Backend.Libs.Mediator.Messaging.Abstractions;
using Backend.Libs.Messaging.Attributes;

namespace Backend.Libs.Messaging.Messages;

[QueueName("test.message")]
public class TestMessage : IMessage
{
    public required Guid Id { get; init; }
    public required string Text { get; init; }
    public required DateTime CreatedAt { get; init; }
}