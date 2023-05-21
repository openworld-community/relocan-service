using Microsoft.Extensions.DependencyInjection;

namespace ReloCAN.Service.SharedKernel.Idempotency;

public sealed class IdempotencyConfigurationBuilder<TUnitOfWork>
  where TUnitOfWork : UnitOfWorkBase
{
  internal IdempotencyConfigurationBuilder(IServiceCollection services)
  {
    Services = services;
  }

  public IServiceCollection Services { get; }
}
