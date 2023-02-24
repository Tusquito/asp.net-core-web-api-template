using System.Text;
using System.Threading.Tasks;
using Backend.Libs.RabbitMQ.Events;
using Backend.Plugins.RabbitMQ.Messages;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;

namespace Backend.Server.RabbitMQ.Events;

public class TestMessageConsumerEventHandler : IAsyncRabbitMqConsumerEventHandler<TestMessage>
{
    private readonly ILogger<TestMessageConsumerEventHandler> _logger;

    public TestMessageConsumerEventHandler(ILogger<TestMessageConsumerEventHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(object sender, BasicDeliverEventArgs @event)
    {
       _logger.LogInformation("[{Scope}] New message consumed: {Message}", nameof(TestMessageConsumerEventHandler), Encoding.UTF8.GetString(@event.Body.ToArray()));
       return Task.CompletedTask;
    }
}