using aliment_backend.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace unit_test.Controller_test
{
    public class RoleControllerTest
    {
        private readonly ApplicationDbContextMoq _contextMock = new();
        private readonly RoleController _controller;
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;

        public RoleControllerTest()
        {
            IQueryable<IdentityRole> list = new List<IdentityRole>()
            {
                new("Admin"),
                new("User")
            }
            .AsQueryable();

            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(
                new Mock<IRoleStore<IdentityRole>>().Object,
                Array.Empty<IRoleValidator<IdentityRole>>(),
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<ILogger<RoleManager<IdentityRole>>>().Object);

            _roleManagerMock
                .Setup(r => r.Roles).Returns(list);

            _controller = new RoleController(_roleManagerMock.Object);
        }

        [Fact]
        public void GetAllUser_ReturnsListOfRoles()
        {
            // Act
            OkObjectResult? result = _controller.GetAllUser() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            List<IdentityRole> roleList = Assert.IsAssignableFrom<List<IdentityRole>>(result.Value);
            Assert.Equal(2, roleList.Count);
            Assert.Contains(roleList, r => r.Name == "Admin");
            Assert.Contains(roleList, r => r.Name == "User");
        }
    }
}
