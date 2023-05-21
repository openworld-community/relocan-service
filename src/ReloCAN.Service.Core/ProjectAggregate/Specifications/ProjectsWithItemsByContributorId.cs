using Ardalis.Specification;

namespace ReloCAN.Service.Core.ProjectAggregate.Specifications;

public class ProjectsWithItemsByUserIdSpec : Specification<Project>, ISingleResultSpecification
{
  public ProjectsWithItemsByUserIdSpec(int userId)
  {
    Query
        .Where(project => project.Items.Any(item => item.UserId == userId))
        .Include(project => project.Items);
  }
}
