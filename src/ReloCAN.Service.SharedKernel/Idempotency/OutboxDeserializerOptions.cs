using System.Reflection;

namespace ReloCAN.Service.SharedKernel.Idempotency;

public sealed class OutboxDeserializerOptions
{
  private readonly ISet<Assembly> _assemblies;

  internal OutboxDeserializerOptions()
  {
    _assemblies = new HashSet<Assembly>
    {
      typeof(string).Assembly
    };
  }

  public IReadOnlyCollection<Assembly> Assemblies => _assemblies.ToArray();

  public void AddAssembly(Assembly assembly)
  {
    _assemblies.Add(assembly);
  }
}
