using Backend.Libs.RabbitMQ;
using Backend.Libs.RabbitMQ.Attributes;

namespace Backend.Plugins.RabbitMQ.Messages;

[QueueName("test.message")]
public class TestMessage : IRabbitMqMessage<TestMessage>
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}