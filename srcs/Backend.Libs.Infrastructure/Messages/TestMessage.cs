using Backend.Libs.Infrastructure.Attributes;
using Backend.Libs.Infrastructure.Messages.Abstractions;

namespace Backend.Libs.Infrastructure.Messages;

[QueueName("test.message")]
public class TestMessage : IMessage
{
    public required Guid Id { get; init; }
    public required string Text { get; init; }
    public required DateTime CreatedAt { get; init; }
}