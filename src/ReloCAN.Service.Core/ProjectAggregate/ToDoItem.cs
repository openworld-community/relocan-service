using Ardalis.GuardClauses;
using ReloCAN.Service.Core.ProjectAggregate.Events;
using ReloCAN.Service.SharedKernel;

namespace ReloCAN.Service.Core.ProjectAggregate;

public class ToDoItem : EntityBase
{
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public int? UserId { get; private set; }
  public bool IsDone { get; private set; }

  public void MarkComplete()
  {
    if (!IsDone)
    {
      IsDone = true;

      RegisterDomainEvent(new ToDoItemCompletedEvent(this));
    }
  }

  public void AddUser(int userId)
  {
    Guard.Against.Null(userId, nameof(userId));
    UserId = userId;

    var userAddedToItemEvent = new UserAddedToItemEvent(this, userId);
    base.RegisterDomainEvent(userAddedToItemEvent);
  }

  public void RemoveUser()
  {
    UserId = null;
  }

  public override string ToString()
  {
    string status = IsDone ? "Done!" : "Not done.";
    return $"{Id}: Status: {status} - {Title} - {Description}";
  }
}
