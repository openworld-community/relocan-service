namespace ReloCAN.Service.SharedKernel.Idempotency;

public interface IOutboxWriteRepository
{
  Task Add(Outbox outbox);
  Task Update(Outbox outbox);
}
