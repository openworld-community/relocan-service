using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ReloCAN.Service.SharedKernel.Idempotency;

    public static class IdempotencyConfigurationBuilderExtensions
    {
        public static IdempotencyConfigurationBuilder<TUnitOfWork> PersistWithEfCore<TUnitOfWork, TDbContext>(
            this IdempotencyConfigurationBuilder<TUnitOfWork> builder,
            Func<IServiceProvider, TDbContext> dbContextFactory)
            where TDbContext : DbContext, IDbContextWithOutbox
            where TUnitOfWork : UnitOfWorkBase<TDbContext>
        {
            return builder.PersistWithEfCore(dbContextFactory, null);
        }

        public static IdempotencyConfigurationBuilder<TUnitOfWork> PersistWithEfCore<TUnitOfWork, TDbContext>(
            this IdempotencyConfigurationBuilder<TUnitOfWork> builder,
            Func<IServiceProvider, TDbContext> dbContextFactory,
            Action<IdempotencyEfCoreOptions<TDbContext, TUnitOfWork>>? optionsBuilder)
            where TDbContext : DbContext, IDbContextWithOutbox
            where TUnitOfWork : UnitOfWorkBase<TDbContext>
        {
            var options = new IdempotencyEfCoreOptions<TDbContext, TUnitOfWork>();

            optionsBuilder?.Invoke(options);

            builder.Services.AddTransient<TUnitOfWork>();
            Func<IServiceProvider, TUnitOfWork> unitOfWorkFactory = s => s.GetRequiredService<TUnitOfWork>();

            builder.Services.AddTransient<IUnitOfWorkFactory<TUnitOfWork>>(c =>
                new UnitOfWorkFactory<TUnitOfWork, TDbContext>(
                    c.GetRequiredService<IOutboxDispatcher>(),
                    () => dbContextFactory.Invoke(c),
                    () => unitOfWorkFactory(c)));

            builder.Services.AddTransient<IOutboxReadRepository>(c =>
                new OutboxReadRepository<TDbContext>(
                    c.GetRequiredService<ILogger<OutboxReadRepository<TDbContext>>>(),
                    () => dbContextFactory.Invoke(c),
                    options.OutboxDeserializer));
            
            return builder;
        }
    }
