using Ardalis.ApiEndpoints;
using Backend.Libs.Domain;
using Backend.Libs.RabbitMQ.Publishers;
using Backend.Plugins.RabbitMQ.Messages;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Tests.Endpoints;

public class TestRabbitMqEndpoint : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult
{
    private readonly ILogger<TestRabbitMqEndpoint> _logger;
    private readonly IRabbitMqPublisher<TestMessage> _publisher;

    public TestRabbitMqEndpoint(ILogger<TestRabbitMqEndpoint> logger, IRabbitMqPublisher<TestMessage> publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }
    [HttpGet("api/test/rabbitmq")]
    public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = new())
    {
        try
        {
            await _publisher.PublishAsync(new TestMessage(), cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(TestRabbitMqEndpoint));
            return DomainResults.InternalServerError();
        }

        return DomainResults.Ok();
    }
}