using System.Collections.Concurrent;

namespace ReloCAN.Service.SharedKernel.Idempotency;

internal class TypesCache
{
  public static TypesCache Instance { get; }

  static TypesCache()
  {
    Instance = new TypesCache();
  }

  private readonly ConcurrentDictionary<string, Type> _cache;

  private TypesCache()
  {
    _cache = new ConcurrentDictionary<string, Type>();
  }

  public Type GetOrAdd(string name, Func<string, Type> resolver)
  {
    return _cache.GetOrAdd(name, resolver);
  }
}
