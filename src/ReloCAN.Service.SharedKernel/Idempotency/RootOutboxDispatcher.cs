using MassTransit;

namespace ReloCAN.Service.SharedKernel.Idempotency;

internal sealed class RootOutboxDispatcher : IOutboxDispatcher
{
  private readonly IBus _bus;

  public RootOutboxDispatcher(IBus bus)
  {
    _bus = bus;
  }

  public Task Send(object command)
  {
    //TODO
    throw new NotImplementedException();
  }

  public Task Publish(object evt)
  {
    return _bus.Publish(evt);
  }
}
