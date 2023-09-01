using Microsoft.EntityFrameworkCore.Storage;

namespace ReloCAN.Service.SharedKernel.Idempotency;

public abstract class UnitOfWorkBase<TDbContext> : UnitOfWorkBase
        where TDbContext : Microsoft.EntityFrameworkCore.DbContext, IDbContextWithOutbox
    {
        private TDbContext? _dbContext;
        private IDbContextTransaction? _transaction;
        
        protected abstract void ProvisionRepositories(TDbContext dbContext);

        /// <summary>
        /// Initializes transactional unit of work.
        /// </summary>
        public async Task Init(IOutboxDispatcher defaultOutboxDispatcher, TDbContext dbContext, Outbox outbox)
        {
            _dbContext = dbContext;
            _transaction = await dbContext.Database.BeginTransactionAsync();

            var outboxWriteRepository = new OutboxWriteRepository<TDbContext>(dbContext);

            ProvisionRepositories(dbContext);

            await Init(defaultOutboxDispatcher, outboxWriteRepository, outbox);
        }

        /// <summary>
        /// Initializes non-transactional unit of work.
        /// </summary>
        public Task Init(TDbContext dbContext)
        {
            _dbContext = dbContext;

            ProvisionRepositories(dbContext);

            return Task.CompletedTask;
        }

        protected override Task CommitImpl()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("UnitOfWork is either not initialized or its non-transactional");
            }

            return _transaction.CommitAsync();
        }

        protected override async Task RollbackImpl()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("UnitOfWork is either not initialized or its non-transactional");
            }

            try
            {
                await _transaction.RollbackAsync();
            }
            // In the case if an NpgsqlException was thrown within the transaction
            // an attempt to roll it back leads to the InvalidOperationException.
            // NOTE: Previously, in Npgsql.EntityFrameworkCore.PostgreSQL v5.0.2,
            // the ObjectDisposedException was used here instead of the InvalidOperationException
            catch (InvalidOperationException e) when (e.Message == "This NpgsqlTransaction has completed; it is no longer usable.")
            {                
            }
        }

        protected override async ValueTask DisposeAsync(bool disposing)
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
            }

            if (_dbContext != null)
            {
                await _dbContext.DisposeAsync();
            }
        }
    }
