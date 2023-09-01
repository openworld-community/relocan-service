using Microsoft.EntityFrameworkCore;

namespace ReloCAN.Service.SharedKernel.Idempotency;

public sealed class IdempotencyEfCoreOptions<TDbContext, TUnitOfWork>
  where TDbContext : DbContext, IDbContextWithOutbox
  where TUnitOfWork : UnitOfWorkBase<TDbContext>
{
  internal IdempotencyEfCoreOptions()
  {
    OutboxDeserializer = new OutboxDeserializerOptions();
  }

  public OutboxDeserializerOptions OutboxDeserializer { get; }
}
