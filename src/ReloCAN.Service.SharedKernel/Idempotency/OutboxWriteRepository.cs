using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ReloCAN.Service.SharedKernel.Idempotency;

internal class OutboxWriteRepository<TDbContext> : IOutboxWriteRepository
  where TDbContext : DbContext, IDbContextWithOutbox
{
  private readonly TDbContext _dbContext;

  public OutboxWriteRepository(TDbContext dbContext)
  {
    _dbContext = dbContext;
  }
        
  public async Task Add(Outbox outbox)
  {
    var entity = MapToEntity(outbox);

    _dbContext.Outbox.Add(entity);

    await _dbContext.SaveChangesAsync();
  }

  public async Task Update(Outbox outbox)
  {
    var entity = MapToEntity(outbox);

    _dbContext.Outbox.Local.Clear();
    _dbContext.Outbox.Update(entity);

    await _dbContext.SaveChangesAsync();
  }

  private static OutboxEntity MapToEntity(Outbox outbox)
  {
    var response = SerializeObject(outbox.Response);
    var commands = SerializeObject(outbox
      .Commands
      ?.Select(SerializeObject)
      .ToArray());
    var events = SerializeObject(outbox
      .Events
      ?.Select(SerializeObject)
      .ToArray());

    return new OutboxEntity
    {
      IdempotencyId = outbox.IdempotencyId,
      IsDispatched = outbox.IsDispatched,
      Response = response,
      Commands = commands,
      Events = events
    };
  }

  private static string? SerializeObject(object? obj)
  {
    if (obj == null)
      return null;
    
    var envelope = new ObjectEnvelope
    {
      Type = obj.GetType().FullName,
      Body = JsonConvert.SerializeObject(obj, new ProtoMessageJsonConverter())
    };

    return JsonConvert.SerializeObject(envelope);
  }
}
