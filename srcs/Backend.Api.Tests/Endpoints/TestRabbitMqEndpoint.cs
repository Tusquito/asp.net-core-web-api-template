using Ardalis.ApiEndpoints;
using Backend.Libs.Domain;
using Backend.Libs.Infrastructure.Messages;
using Backend.Libs.Infrastructure.Producers.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Tests.Endpoints;

public class TestRabbitMqEndpoint : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult
{
    private readonly ILogger<TestRabbitMqEndpoint> _logger;
    private readonly IMessageProducer _producer;

    public TestRabbitMqEndpoint(ILogger<TestRabbitMqEndpoint> logger, IMessageProducer producer)
    {
        _logger = logger;
        _producer = producer;
    }
    
    [HttpGet("rabbitmq")]
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
            return DomainResult.InternalServerError();
        }

        return DomainResult.Ok();
    }
}