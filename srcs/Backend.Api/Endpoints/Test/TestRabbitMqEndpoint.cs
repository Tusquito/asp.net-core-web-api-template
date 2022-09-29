using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Backend.Libs.Domain;
using Backend.Libs.RabbitMQ.Producers;
using Backend.Plugins.RabbitMQ.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Backend.Api.Endpoints.Test;

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
            await _producer.PublishAsync(new TestMessage(), cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(TestRabbitMqEndpoint));
            return EndpointResult.InternalServerError();
        }

        return EndpointResult.Ok();
    }
}