namespace ReloCAN.Service.SharedKernel.Idempotency;

public interface IOutboxDispatcher
{
  Task Send(object command);

  Task Publish(object evt);
}
