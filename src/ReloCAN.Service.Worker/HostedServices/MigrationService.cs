using Microsoft.EntityFrameworkCore;
using ReloCAN.Service.Infrastructure.Data;

namespace ReloCAN.Service.Worker.HostedServices;

public class MigrationService : IHostedService
{
  private readonly ILogger<MigrationService> _logger;
  private readonly AppDbContext _appDbContext;

  public MigrationService(AppDbContext appDbContext, ILogger<MigrationService> logger)
  {
    _appDbContext = appDbContext;
    _logger = logger;
  }

  public async Task StartAsync(CancellationToken cancellationToken)
  {
    _logger.LogInformation("Started applying pending migrations.");
    await _appDbContext.Database.MigrateAsync(cancellationToken);
    _logger.LogInformation("Finished applying pending migrations.");
  }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    return Task.CompletedTask;
  }
}
