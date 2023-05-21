using System.Reflection;
using ReloCAN.Service.Core.ProjectAggregate;
using ReloCAN.Service.SharedKernel;
using ReloCAN.Service.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using ReloCAN.Service.Core.UserAggregate;
using ReloCAN.Service.SharedKernel.Idempotency;

namespace ReloCAN.Service.Infrastructure.Data;

public class AppDbContext : DbContext, IDbContextWithOutbox
{
  private readonly IDomainEventDispatcher? _dispatcher;
  
  public static string SchemaName { get; } = "Relocan";
  public static string MigrationTable { get; } = "__EFMigrationsHistory";

  public AppDbContext(DbContextOptions<AppDbContext> options,
    IDomainEventDispatcher? dispatcher)
      : base(options)
  {
    _dispatcher = dispatcher;
  }

  public DbSet<ToDoItem> ToDoItems => Set<ToDoItem>();
  public DbSet<Project> Projects => Set<Project>();
  public DbSet<User> Users => Set<User>();
  public DbSet<OutboxEntity> Outbox => Set<OutboxEntity>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.HasDefaultSchema(SchemaName);
    
    modelBuilder.Entity<OutboxEntity>()
      .ToTable("Outbox")
      .HasKey(x => x.IdempotencyId);
    
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
  {
    int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

    // ignore events if no dispatcher provided
    if (_dispatcher == null) return result;

    // dispatch events only if save was successful
    var entitiesWithEvents = ChangeTracker.Entries<EntityBase>()
        .Select(e => e.Entity)
        .Where(e => e.DomainEvents.Any())
        .ToArray();

    await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

    return result;
  }

  public override int SaveChanges()
  {
    return SaveChangesAsync().GetAwaiter().GetResult();
  }
}
