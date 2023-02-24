using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Backend.Libs.Mediator.Messaging.Abstractions;
using Backend.Plugins.Messaging.Messages;
using Microsoft.Extensions.Logging;

namespace Backend.Server.Messaging.Events;

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