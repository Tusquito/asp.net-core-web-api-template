﻿using Backend.Libs.Mediator.Messaging.Abstractions;
using Backend.Libs.Messaging.Attributes;

namespace Backend.Plugins.Messaging.Messages;

[QueueName("test.message")]
public class TestMessage : IMessage<TestMessage>
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}