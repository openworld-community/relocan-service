using Ardalis.Result;
using ReloCAN.Service.Core.Interfaces;
using ReloCAN.Service.SharedKernel.Interfaces;
using MediatR;
using ReloCAN.Service.Core.UserAggregate;
using ReloCAN.Service.Core.UserAggregate.Events;

namespace ReloCAN.Service.Core.Services;

public class DeleteUserService : IDeleteUserService
{
  private readonly IRepository<User> _repository;
  private readonly IMediator _mediator;

  public DeleteUserService(IRepository<User> repository, IMediator mediator)
  {
    _repository = repository;
    _mediator = mediator;
  }

  public async Task<Result> DeleteUser(int userId)
  {
    var aggregateToDelete = await _repository.GetByIdAsync(userId);
    if (aggregateToDelete == null) return Result.NotFound();

    await _repository.DeleteAsync(aggregateToDelete);
    var domainEvent = new UserDeletedEvent(userId);
    await _mediator.Publish(domainEvent);
    return Result.Success();
  }
}
