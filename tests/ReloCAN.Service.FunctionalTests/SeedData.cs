using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReloCAN.Service.Core.ProjectAggregate;
using ReloCAN.Service.Core.UserAggregate;
using ReloCAN.Service.Infrastructure.Data;

namespace ReloCAN.Service.FunctionalTests;

public static class SeedData
{
  public static readonly User User1 = new ("John");
  public static readonly User User2 = new ("Doe");
  public static readonly Project TestProject1 = new Project("Test Project", PriorityStatus.Backlog);
  public static readonly ToDoItem ToDoItem1 = new ToDoItem
  {
    Title = "Get Sample Working",
    Description = "Try to get the sample to build."
  };
  public static readonly ToDoItem ToDoItem2 = new ToDoItem
  {
    Title = "Review Solution",
    Description = "Review the different projects in the solution and how they relate to one another."
  };
  public static readonly ToDoItem ToDoItem3 = new ToDoItem
  {
    Title = "Run and Review Tests",
    Description = "Make sure all the tests run and review what they are doing."
  };

  public static void Initialize(IServiceProvider serviceProvider)
  {
    using (var dbContext = new AppDbContext(
        serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>(), null))
    {
      // Look for any TODO items.
      if (dbContext.ToDoItems.Any())
      {
        return;   // DB has been seeded
      }

      PopulateTestData(dbContext);


    }
  }
  public static void PopulateTestData(AppDbContext dbContext)
  {
    foreach (var item in dbContext.Projects)
    {
      dbContext.Remove(item);
    }
    foreach (var item in dbContext.ToDoItems)
    {
      dbContext.Remove(item);
    }
    foreach (var item in dbContext.Users)
    {
      dbContext.Remove(item);
    }
    dbContext.SaveChanges();

    dbContext.Users.Add(User1);
    dbContext.Users.Add(User2);

    dbContext.SaveChanges();

    ToDoItem1.AddUser(User1.Id);
    ToDoItem2.AddUser(User2.Id);
    ToDoItem3.AddUser(User1.Id);

    TestProject1.AddItem(ToDoItem1);
    TestProject1.AddItem(ToDoItem2);
    TestProject1.AddItem(ToDoItem3);
    dbContext.Projects.Add(TestProject1);

    dbContext.SaveChanges();
  }
}
