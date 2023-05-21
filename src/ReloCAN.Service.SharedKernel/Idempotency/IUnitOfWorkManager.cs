namespace ReloCAN.Service.SharedKernel.Idempotency;

public interface IUnitOfWorkManager<TUnitOfWork> where TUnitOfWork : UnitOfWorkBase
{
  /// <summary>
  /// Begins the transactional unit of work
  /// </summary>
  Task<TUnitOfWork> Begin(string idempotencyId);

  /// <summary>
  /// Begins either non-transactional unit of work
  /// </summary>
  Task<TUnitOfWork> Begin();
}
