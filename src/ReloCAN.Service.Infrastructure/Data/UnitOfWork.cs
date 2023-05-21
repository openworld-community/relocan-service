using ReloCAN.Service.Core.UserAggregate;
using ReloCAN.Service.SharedKernel.Idempotency;
using ReloCAN.Service.SharedKernel.Interfaces;

namespace ReloCAN.Service.Infrastructure.Data;

public class UnitOfWork : UnitOfWorkBase<AppDbContext>
{
  private IRepository<User>? _users;

  public IRepository<User> Users
  {
    get => _users ?? throw new InvalidOperationException("Uninitialized UoW!");
  }

  protected override void ProvisionRepositories(AppDbContext dbContext)
  {
    _users = new EfRepository<User>(dbContext);
  }
}
