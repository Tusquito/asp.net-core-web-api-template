using Ardalis.ApiEndpoints;
using Backend.Libs.Domain;
using Backend.Libs.Messaging.Abstractions;
using Backend.Libs.Messaging.Messages;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Tests.Endpoints;

public class TestRabbitMqEndpoint : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult
{
    private readonly ILogger<TestRabbitMqEndpoint> _logger;
    private readonly IMessageProducer<TestMessage> _producer;

    public TestRabbitMqEndpoint(ILogger<TestRabbitMqEndpoint> logger, IMessageProducer<TestMessage> producer)
    {
        _logger = logger;
        _producer = producer;
    }
    [HttpGet("api/test/rabbitmq")]
    public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = new())
    {
        try
        {
            await _producer.ProduceAsync(new TestMessage
            {
                Id = Guid.NewGuid(),
                Text = "",
                CreatedAt = DateTime.UtcNow
            }, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(TestRabbitMqEndpoint));
            return DomainResults.InternalServerError();
        }

        return DomainResults.Ok();
    }
}