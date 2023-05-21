using Microsoft.EntityFrameworkCore;

namespace ReloCAN.Service.SharedKernel.Idempotency;

internal sealed class UnitOfWorkFactory<TUnitOfWork, TDbContext> : IUnitOfWorkFactory<TUnitOfWork>
  where TUnitOfWork : UnitOfWorkBase<TDbContext>
  where TDbContext : DbContext, IDbContextWithOutbox
{
  private readonly IOutboxDispatcher _defaultOutboxDispatcher;
  private readonly Func<TDbContext> _dbContextFactory;
  private readonly Func<TUnitOfWork> _unitOfWorkFactory;

  public UnitOfWorkFactory(IOutboxDispatcher defaultOutboxDispatcher, 
    Func<TDbContext> dbContextFactory,
    Func<TUnitOfWork> unitOfWorkFactory)
  {
    _defaultOutboxDispatcher = defaultOutboxDispatcher;
    _dbContextFactory = dbContextFactory;
    _unitOfWorkFactory = unitOfWorkFactory;
  }

  public async Task<TUnitOfWork> Create(Outbox outbox)
  {
    var dbContext = _dbContextFactory.Invoke();
    var unitOfWork = _unitOfWorkFactory.Invoke();

    await unitOfWork.Init(_defaultOutboxDispatcher, dbContext, outbox);

    return unitOfWork;
  }

  public async Task<TUnitOfWork> Create()
  {
    var dbContext = _dbContextFactory.Invoke();
    var unitOfWork = _unitOfWorkFactory.Invoke();

    await unitOfWork.Init(dbContext);

    return unitOfWork;
            
  }
}
