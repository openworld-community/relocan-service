using ReloCAN.Service.Core.UserAggregate;
using Xunit;

namespace ReloCAN.Service.UnitTests.Core.UserAggregate;

public class UserConstructor
{
  private readonly string _testName = "test name";
  private User? _testUser;

  private User CreateUser()
  {
    return new User(_testName);
  }

  [Fact]
  public void InitializesName()
  {
    _testUser = CreateUser();

    Assert.Equal(_testName, _testUser.Name);
  }
}
