using ReloCAN.Service.SharedKernel;

namespace ReloCAN.Service.Core.ProjectAggregate.Events;

public class UserAddedToItemEvent : DomainEventBase
{
  public int UserId { get; set; }
  public ToDoItem Item { get; set; }

  public UserAddedToItemEvent(ToDoItem item, int userId)
  {
    Item = item;
    UserId = userId;
  }
}
