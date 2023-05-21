using ReloCAN.Service.Core.Services;
using ReloCAN.Service.SharedKernel.Interfaces;
using MediatR;
using Moq;
using ReloCAN.Service.Core.UserAggregate;
using Xunit;

namespace ReloCAN.Service.UnitTests.Core.Services
{
    public class DeleteUserService_DeleteUser
    {
        private readonly Mock<IRepository<User>> _mockRepo = new Mock<IRepository<User>>();
        private readonly Mock<IMediator> _mockMediator = new Mock<IMediator>();
        private readonly DeleteUserService _service;

        public DeleteUserService_DeleteUser()
        {
            _service = new DeleteUserService(_mockRepo.Object, _mockMediator.Object);
        }

        [Fact]
        public async Task ReturnsNotFoundGivenCantFindUser()
        {
            var result = await _service.DeleteUser(0);

            Assert.Equal(Ardalis.Result.ResultStatus.NotFound, result.Status);
        }
    }
}
