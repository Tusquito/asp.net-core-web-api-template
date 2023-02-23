using System.Threading;
using System.Threading.Tasks;
using Backend.Libs.RabbitMQ.Handlers;
using Backend.Plugins.RabbitMQ.Messages;
using Microsoft.Extensions.Logging;

namespace Backend.Server.RabbitMQ.Handlers;

public class TestMessageConsumerMessageHandler : IAsyncRabbitMqConsumerMessageHandler<TestMessage>
{
    private readonly ILogger<TestMessageConsumerMessageHandler> _logger;

    public TestMessageConsumerMessageHandler(ILogger<TestMessageConsumerMessageHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(TestMessage message, CancellationToken cancellationToken)
    {
       _logger.LogInformation("[{Scope}] New message consumed: {Message}", nameof(TestMessageConsumerMessageHandler), message);
       return Task.CompletedTask;
    }
}