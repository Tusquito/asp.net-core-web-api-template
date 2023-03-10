using System.Text.Json;
using Backend.Libs.Infrastructure.Handlers.Abstractions;
using Backend.Libs.Infrastructure.Messages;
using Microsoft.Extensions.Logging;

namespace Backend.Libs.Infrastructure.Handlers;

public class TestMessageEventHandler : IAsyncEventHandler<TestMessage>
{
    private readonly ILogger<TestMessageEventHandler> _logger;

    public TestMessageEventHandler(ILogger<TestMessageEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(TestMessage request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[{Scope}] New message consumed: {Message}", nameof(TestMessageEventHandler), JsonSerializer.Serialize(request));
        return Task.CompletedTask;
    }
}