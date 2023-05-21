namespace ReloCAN.Service.SharedKernel.Idempotency;

internal interface IOutboxManager
{
  Task<Outbox> Open(string idempotencyId);
}
