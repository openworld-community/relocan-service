using ReloCAN.Service.SharedKernel;

namespace ReloCAN.Service.Core.UserAggregate.Events;

public class UserDeletedEvent : DomainEventBase
{
  public int UserId { get; set; }

  public UserDeletedEvent(int userId)
  {
    UserId = userId;
  }
}
