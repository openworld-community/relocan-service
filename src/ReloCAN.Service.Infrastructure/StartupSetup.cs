using ReloCAN.Service.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ReloCAN.Service.Infrastructure;

public static class StartupSetup
{
  public static void AddDbContext(this IServiceCollection services, string connectionString)
  {
    services.AddDbContext<AppDbContext>(options =>
      options.UseNpgsql(connectionString, builder =>
      {
        builder.MigrationsHistoryTable(
          AppDbContext.MigrationTable,
          AppDbContext.SchemaName);
      }));
    
    services.AddSingleton<DbContextOptionsBuilder<AppDbContext>>(c => CreateDbContextOptionsBuilder(connectionString, 
      c.GetService<ILoggerFactory>()));
  }
  
  private static DbContextOptionsBuilder<AppDbContext> CreateDbContextOptionsBuilder(string connectionString,
    ILoggerFactory? loggerFactory)
  {
    var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
    optionsBuilder.UseLoggerFactory(loggerFactory);

    optionsBuilder.UseNpgsql(connectionString,
      builder =>
        builder.MigrationsHistoryTable(
          AppDbContext.MigrationTable,
          AppDbContext.SchemaName));
    return optionsBuilder;
  }
}
