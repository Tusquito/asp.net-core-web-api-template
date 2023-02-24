using Ardalis.ApiEndpoints;
using Backend.Libs.Domain;
using Backend.Libs.Messaging.Producers;
using Backend.Plugins.Messaging.Messages;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Tests.Endpoints;

public class TestRabbitMqEndpoint : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult
{
    private readonly ILogger<TestRabbitMqEndpoint> _logger;
    private readonly IRabbitMqProducer<TestMessage> _producer;

    public TestRabbitMqEndpoint(ILogger<TestRabbitMqEndpoint> logger, IRabbitMqProducer<TestMessage> producer)
    {
        _logger = logger;
        _producer = producer;
    }
    [HttpGet("api/test/rabbitmq")]
    public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = new())
    {
        try
        {
            await _producer.ProduceAsync(new TestMessage(), cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(TestRabbitMqEndpoint));
            return DomainResults.InternalServerError();
        }

        return DomainResults.Ok();
    }
}