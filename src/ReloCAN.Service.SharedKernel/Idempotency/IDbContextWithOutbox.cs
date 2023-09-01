using Microsoft.EntityFrameworkCore;

namespace ReloCAN.Service.SharedKernel.Idempotency;

public interface IDbContextWithOutbox
{
  DbSet<OutboxEntity> Outbox { get; }
}
