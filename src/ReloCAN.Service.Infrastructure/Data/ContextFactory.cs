using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ReloCAN.Service.Infrastructure.Data;

/// <summary>
/// 
/// </summary>
[UsedImplicitly]
public class ContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
  public AppDbContext CreateDbContext(string[] args)
  {
    var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
    optionsBuilder.UseNpgsql("unused");

    return new AppDbContext(optionsBuilder.Options, null);
  }
}
