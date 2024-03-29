namespace ReloCAN.Service.SharedKernel.Idempotency;

public interface IUnitOfWorkFactory<TUnitOfWork>
  where TUnitOfWork : UnitOfWorkBase
{
  /// <summary>
  /// Should create transactional unit of work
  /// </summary>
  Task<TUnitOfWork> Create(Outbox outbox);

  /// <summary>
  /// Should create non-transactional unit of work
  /// </summary>
  Task<TUnitOfWork> Create();
}
