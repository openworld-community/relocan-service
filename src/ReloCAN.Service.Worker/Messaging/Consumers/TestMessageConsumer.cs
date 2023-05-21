using MassTransit;
using ReloCAN.Service.SharedKernel.Messages;

namespace ReloCAN.Service.Worker.Messaging.Consumers;

public class TestMessageConsumer : IConsumer<TestMessage>
{
  private readonly ILogger<TestMessageConsumer> _logger;

  public TestMessageConsumer(ILogger<TestMessageConsumer> logger)
  {
    _logger = logger;
  }

  public Task Consume(ConsumeContext<TestMessage> context)
  {
    _logger.LogInformation("Test message is being consumed {@context}", new
    {
      Message = context.Message
    });
    return Task.CompletedTask;
  }
}
