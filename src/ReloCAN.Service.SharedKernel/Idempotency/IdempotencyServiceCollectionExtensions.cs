using Microsoft.Extensions.DependencyInjection;

namespace ReloCAN.Service.SharedKernel.Idempotency;

public static class IdempotencyServiceCollectionExtensions
{
  public static IServiceCollection AddIdempotency<TUnitOfWork>(this IServiceCollection services,
    Action<IdempotencyConfigurationBuilder<TUnitOfWork>>? config)
    where TUnitOfWork : UnitOfWorkBase
  {
    services.AddTransient<IUnitOfWorkManager<TUnitOfWork>, UnitOfWorkManager<TUnitOfWork>>();

    services.AddTransient<IOutboxManager, OutboxManager>();

    if (config != null)
    {
      var configBuilder = new IdempotencyConfigurationBuilder<TUnitOfWork>(services);

      config.Invoke(configBuilder);
    }

    return services;
  }
}
