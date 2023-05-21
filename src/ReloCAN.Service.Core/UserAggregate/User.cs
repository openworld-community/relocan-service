using Ardalis.GuardClauses;
using ReloCAN.Service.SharedKernel;
using ReloCAN.Service.SharedKernel.Interfaces;

namespace ReloCAN.Service.Core.UserAggregate;

public class User : EntityBase, IAggregateRoot
{
  public string Name { get; private set; }

  public User(string name)
  {
    Name = Guard.Against.NullOrEmpty(name, nameof(name));
  }

  public void UpdateName(string newName)
  {
    Name = Guard.Against.NullOrEmpty(newName, nameof(newName));
  }
}
