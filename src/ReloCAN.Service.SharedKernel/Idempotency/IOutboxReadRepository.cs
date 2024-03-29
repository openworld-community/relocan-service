namespace ReloCAN.Service.SharedKernel.Idempotency;

/// <summary>
/// Should work out of the context of the <see cref="IUnitOfWork"/> transaction
/// </summary>
public interface IOutboxReadRepository
{
  Task<Outbox?> GetOrDefault(string idempotencyId);
}
