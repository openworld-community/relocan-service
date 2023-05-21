using ReloCAN.Service.SharedKernel.Interfaces;
using ReloCAN.Service.Core.ProjectAggregate.Specifications;
using MediatR;
using ReloCAN.Service.Core.UserAggregate.Events;

namespace ReloCAN.Service.Core.ProjectAggregate.Handlers;

public class UserDeletedHandler : INotificationHandler<UserDeletedEvent>
{
  private readonly IRepository<Project> _repository;

  public UserDeletedHandler(IRepository<Project> repository)
  {
    _repository = repository;
  }

  public async Task Handle(UserDeletedEvent domainEvent, CancellationToken cancellationToken)
  {
    var projectSpec = new ProjectsWithItemsByUserIdSpec(domainEvent.UserId);
    var projects = await _repository.ListAsync(projectSpec, cancellationToken);
    foreach (var project in projects)
    {
      project.Items
        .Where(item => item.UserId == domainEvent.UserId)
        .ToList()
        .ForEach(item => item.RemoveUser());
      await _repository.UpdateAsync(project, cancellationToken);
    }
  }
}
