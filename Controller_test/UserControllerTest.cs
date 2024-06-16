using aliment_backend.Controllers;
using aliment_backend.Entities;
using aliment_backend;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using aliment_backend.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using aliment_backend.DTOs;
using System.Reflection;

namespace unit_test.Controller_test
{
    public class UserControllerTest
    {
        private readonly ApplicationDbContextMoq _contextMock = new();
        private readonly UnitOfWorkClass _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserController _controller;
        private readonly Mock<UserManager<User>> _userManagerMock;

        public UserControllerTest()
        {
            _userManagerMock = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                Array.Empty<IUserValidator<User>>(),
                Array.Empty<IPasswordValidator<User>>(),
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object
            );

            MapperConfiguration mapperConfig = new(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            _mapper = mapperConfig.CreateMapper();
            _unitOfWork = new UnitOfWorkClass(_contextMock.Context);
            _controller = new UserController(_unitOfWork, _mapper, _userManagerMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_Returns_OkResult_With_UsersAndRoles()
        {
            // Arrange
            if (_contextMock.Context.Users != null)
            {
                List<User> users = await _contextMock.Context.Users.ToListAsync();
                List<object> usersWithRoles = new();
                _userManagerMock.Setup(x => x.GetUsersInRoleAsync(It.IsAny<string>()))
                                .ReturnsAsync(users);
                foreach (var user in users)
                {
                    _userManagerMock.Setup(mock => mock.GetRolesAsync(user))
                                    .ReturnsAsync(new List<string> { "User" });
                }
                foreach (var user in users)
                {
                    IList<string> roles = await _userManagerMock.Object.GetRolesAsync(user);
                    usersWithRoles.Add(new
                    {
                        user.Firstname,
                        user.Lastname,
                        user.Email,
                        user.UserName,
                        Role = "User"
                    });
                }

                // Act
                IActionResult result = await _controller.GetAllAsync();

                // Assert
                OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
                List<object> usersWithRolesReturned = Assert.IsAssignableFrom<List<object>>(okResult.Value);
                Assert.Equal(usersWithRoles.Count, usersWithRolesReturned.Count);
            }  
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsUser()
        {
            // Arrange
            string? id = "1";
            if (_contextMock.Context.Users != null)
            {
                User? user = await _contextMock.Context.Users.FirstOrDefaultAsync(u => u.Id == id);
                Assert.NotNull(user);

                if (user != null)
                {
                    _userManagerMock.Setup(m => m.FindByIdAsync(id)).ReturnsAsync(user);
                    _userManagerMock.Setup(mock => mock.GetRolesAsync(user))
                              .ReturnsAsync(new List<string> { "Admin" });
                }

                var userWithRole = new
                {
                    user?.Id,
                    user?.Firstname,
                    user?.Lastname,
                    user?.Email,
                    user?.UserName,
                    Role = "Admin"
                };

                // Act
                IActionResult result = await _controller.GetByIdAsync(id);

                // Assert
                OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
                object userWithRoleReturned = Assert.IsAssignableFrom<object>(okResult.Value);
                Dictionary<string, string?> userProperties = new();

                foreach (var property in userWithRoleReturned.GetType().GetProperties())
                {
                    userProperties[property.Name] = Convert.ToString(property.GetValue(userWithRoleReturned));
                }
                Assert.Equal(userWithRole.Firstname, userProperties["Firstname"]);
                Assert.Equal(userWithRole.Lastname, userProperties["Lastname"]);
                Assert.Equal(userWithRole.Email, userProperties["Email"]);
                Assert.Equal(userWithRole.UserName, userProperties["UserName"]);
                Assert.Equal(userWithRole.Role, userProperties["Role"]);
            }            
        }

        [Fact]
        public async Task GetByIdAsync_WithNullId_ReturnsBadRequest()
        {
            // Act
            IActionResult result = await _controller.GetByIdAsync(null!);

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("L'ID est nul ou vide.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetByIdAsync_WithEmptyId_ReturnsBadRequest()
        {
            // Act
            IActionResult result = await _controller.GetByIdAsync(string.Empty);

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("L'ID est nul ou vide.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            string nonExistingId = "45";

            // Act
            IActionResult result = await _controller.GetByIdAsync(nonExistingId);

            // Assert
            NotFoundObjectResult notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            string message = Assert.IsType<string>(notFoundResult.Value);
            Assert.Equal($"L'utilisateur avec l'identifiant {nonExistingId} non trouvé.", message);  
        }

        [Fact]
        public async Task Delete_ExistingUser_ReturnsTrue()
        {
            // Arrange
            string? id = "1";

            if (_contextMock.Context.Users != null) 
            {
                User? user = await _contextMock.Context.Users.FirstOrDefaultAsync(u => u.Id == id);
                Assert.NotNull(user);

                if (user != null)
                {
                    _userManagerMock.Setup(m => m.FindByIdAsync(id)).ReturnsAsync(user);
                    _userManagerMock.Setup(m => m.GetClaimsAsync(user)).ReturnsAsync(new List<Claim>
                    {
                        new(ClaimTypes.Email, user.Email),
                        new("Username", user.UserName)
                    });
                    _userManagerMock.Setup(mock => mock.GetRolesAsync(user))
                               .ReturnsAsync(new List<string> { "Admin" });

                    // Act
                    ActionResult<bool> result = await _controller.Delete(id);

                    // Assert
                    Assert.IsType<ActionResult<bool>>(result);
                    Assert.True(result.Value);

                    _userManagerMock.Verify(m => m.RemoveClaimsAsync(user, It.IsAny<IEnumerable<Claim>>()), Times.Once());
                    _userManagerMock.Verify(m => m.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>()), Times.Once());
                }
            }            
        }

        [Fact]
        public async Task Delete_NonExistingUser_NotFound()
        {
            // Arrange
            string id = "45";

            // Act
            ActionResult<bool> result = await _controller.Delete(id);

            Assert.IsType<ActionResult<bool>>(result); 
            Assert.False(result.Value);

            _userManagerMock.Verify(m => m.RemoveClaimsAsync(It.IsAny<User>(), It.IsAny<IEnumerable<Claim>>()), Times.Never());
            _userManagerMock.Verify(m => m.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()), Times.Never());
        }

        [Fact]
        public async Task UpdateUserAsync_ExistingUser_ReturnsUpdatedUser()
        {
            // Arrange
            string id = "1";

            UserDTO userDTO = new()
            {
                Firstname = "John",
                Lastname = "Doe",
                UserName = "john",
                Email = "john@gmail.com",
                Role = "Admin"
            };

            if (_contextMock.Context.Users != null)
            {
                User? user = await _contextMock.Context.Users.FirstOrDefaultAsync(u => u.Id == id);
                Assert.NotNull(user);

                if (user != null)
                {
                    _userManagerMock.Setup(m => m.FindByIdAsync(id)).ReturnsAsync(user);
                    _userManagerMock.Setup(m => m.GetClaimsAsync(user)).ReturnsAsync(new List<Claim>
                    {
                        new(ClaimTypes.Email, user.Email),
                        new("Username", user.UserName)   ,
                        new(ClaimTypes.Role, "User")
                    });
                    _userManagerMock.Setup(mock => mock.GetRolesAsync(user))
                               .ReturnsAsync(new string[] { "Admin" });
                }

                // Act
                IActionResult result = await _controller.UpdateUserAsync(id, userDTO);

                // Assert
                OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
                object? updatedUser = okResult.Value;
                Assert.NotNull(updatedUser);

                if (updatedUser != null)
                {
                    PropertyInfo[] properties = updatedUser.GetType().GetProperties();
                    object? firstname = properties.FirstOrDefault(p => p.Name == "Firstname")?.GetValue(updatedUser);
                    object? lastname = properties.FirstOrDefault(p => p.Name == "Lastname")?.GetValue(updatedUser);
                    object? email = properties.FirstOrDefault(p => p.Name == "Email")?.GetValue(updatedUser);
                    object? username = properties.FirstOrDefault(p => p.Name == "UserName")?.GetValue(updatedUser);
                    object? role = properties.FirstOrDefault(p => p.Name == "Role")?.GetValue(updatedUser);

                    Assert.Equal(userDTO.Firstname, firstname);
                    Assert.Equal(userDTO.Lastname, lastname);
                    Assert.Equal(userDTO.Email, email);
                    Assert.Equal(userDTO.UserName, username);
                    Assert.Equal(userDTO.Role, role);
                }

                if (user != null)
                {
                    _userManagerMock.Verify(m => m.GetClaimsAsync(user), Times.Once);
                    _userManagerMock.Verify(m => m.ReplaceClaimAsync(user, It.IsAny<Claim>(), It.IsAny<Claim>()), Times.Exactly(3));
                }
            }            
        }        

        [Fact]
        public async Task UpdateUserAsync_UserNameIsNull_EmailIsNull_RoleIsNull_ReturnsOk()
        {
            // Arrange
            string id = "1";
            UserDTO userDTO = new()
            {
                Firstname = "John",
                Lastname = "Doe",
                UserName = null,  
                Email = null,     
                Role = null
            };

            if (_contextMock.Context.Users != null)
            {
                User? user = await _contextMock.Context.Users.FirstOrDefaultAsync(u => u.Id == id);
                Assert.NotNull(user);

                if (user != null)
                {
                    user.UserName = "johndoe"; 
                    user.Email = "john@example.com"; 
                    _userManagerMock.Setup(m => m.FindByIdAsync(id)).ReturnsAsync(user);
                    _userManagerMock.Setup(m => m.GetClaimsAsync(user)).ReturnsAsync(new List<Claim>
                    {
                        new(ClaimTypes.Email, user.Email),
                        new("Username", user.UserName),
                        new(ClaimTypes.Role, "User")
                    });
                    _userManagerMock.Setup(mock => mock.GetRolesAsync(user))
                              .ReturnsAsync(new List<string> { "User" });
                }

                // Act
                IActionResult result = await _controller.UpdateUserAsync(id, userDTO);

                // Assert
                OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
                object? updatedUser = okResult.Value;
                Assert.NotNull(updatedUser);

                if (updatedUser != null)
                {
                    PropertyInfo[] properties = updatedUser.GetType().GetProperties();
                    object? username = properties.FirstOrDefault(p => p.Name == "UserName")?.GetValue(updatedUser);
                    object? email = properties.FirstOrDefault(p => p.Name == "Email")?.GetValue(updatedUser);
                    Assert.Equal(user?.UserName, username);  
                    Assert.Equal(user?.Email, email);
                }

                if (user != null)
                {
                    _userManagerMock.Verify(m => m.GetClaimsAsync(user), Times.Once);
                    _userManagerMock.Verify(m => m.ReplaceClaimAsync(user, It.IsAny<Claim>(), It.IsAny<Claim>()), Times.Never);
                    _userManagerMock.Verify(m => m.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>()), Times.Never);
                    _userManagerMock.Verify(m => m.AddToRoleAsync(user, It.IsAny<string>()), Times.Never);
                    _userManagerMock.Verify(m => m.UpdateAsync(user), Times.Once);
                    _userManagerMock.Verify(m => m.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>()), Times.Never);
                    _userManagerMock.Verify(m => m.AddToRoleAsync(user, It.IsAny<string>()), Times.Never);
                    _userManagerMock.Verify(m => m.ReplaceClaimAsync(user, It.IsAny<Claim>(), It.IsAny<Claim>()), Times.Never);
                    _userManagerMock.Verify(m => m.GetRolesAsync(user), Times.Once);
                }
            }
        }

        [Fact]
        public async Task Update_NonExistingUser_NotFound()
        {
            // Arrange
            string id = "45";

            UserDTO userDTO = new()
            {
                Firstname = "John",
                Lastname = "Doe",
                UserName = "john",
                Email = "john@gmail.com"
            };
            _userManagerMock.Setup(m => m.FindByIdAsync(id)).ReturnsAsync((User)null!);

            // Act & Assert
            IActionResult result = await _controller.UpdateUserAsync(id, userDTO);
            NotFoundObjectResult notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            string message = Assert.IsType<string>(notFoundResult.Value);
            Assert.Equal($"L'utilisateur avec l'identifiant {id} non trouvé.", message);

            _userManagerMock.Verify(m => m.GetClaimsAsync(It.IsAny<User>()), Times.Never);
            _userManagerMock.Verify(m => m.ReplaceClaimAsync(It.IsAny<User>(), It.IsAny<Claim>(), It.IsAny<Claim>()), Times.Never);
        }
    }
}
