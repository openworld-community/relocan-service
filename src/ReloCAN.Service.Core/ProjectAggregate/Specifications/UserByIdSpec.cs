using Ardalis.Specification;
using ReloCAN.Service.Core.UserAggregate;

namespace ReloCAN.Service.Core.ProjectAggregate.Specifications;

public class UserByIdSpec : Specification<User>, ISingleResultSpecification
{
  public UserByIdSpec(int userId)
  {
    Query
        .Where(user => user.Id == userId);
  }
}
