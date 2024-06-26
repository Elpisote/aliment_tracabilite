using aliment_backend.Controllers;
using aliment_backend.Entities;
using aliment_backend.Interfaces;
using aliment_backend.Mail;
using aliment_backend.Models.Authentication;
using aliment_backend.Models.Authentication.Password;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace unit_test.Controller_test
{
    public class AuthControllerTest
    {
        private readonly ApplicationDbContextMoq _contextMock = new();
        private readonly AuthController _controller;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly Mock<IEmailSender> _emailSenderMock;

        public AuthControllerTest()
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

            _authServiceMock = new Mock<IAuthService>();
            _emailSenderMock = new Mock<IEmailSender>();
            _controller = new AuthController(_authServiceMock.Object, _userManagerMock.Object, _emailSenderMock.Object);
        }

        [Fact]
        public async Task Login_ValidLogin_ReturnsOk()
        {
            // Arrange
            Login loginData = new() { Email = "essai@exemple.fr", Password = "Password123!" };

            // Mock le comportement de AuthService
            ResponseToken mockLoginResult = new() { IsSucceed = true, AccessToken = "accessToken", RefreshToken = "refreshToken" };
            _authServiceMock.Setup(service => service.LoginAsync(loginData)).ReturnsAsync(mockLoginResult);

            // Act
            IActionResult result = await _controller.Login(loginData);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            OkObjectResult? okResult = result as OkObjectResult;
            Assert.Equal(mockLoginResult, okResult?.Value);
        }

        [Fact]
        public async Task Login_InvalidLogin_ReturnsUnauthorized()
        {
            // Arrange    
            Login loginData = new() { Email = "essai@exemple.fr", Password = "Password123!" };

            // Mock the behavior of AuthService
            ResponseToken mockLoginResult = new() { IsSucceed = false, AccessToken = "", RefreshToken = "" };
            _authServiceMock.Setup(service => service.LoginAsync(loginData)).ReturnsAsync(mockLoginResult);

            // Act
            IActionResult result = await _controller.Login(loginData);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
            UnauthorizedObjectResult? unauthorizedResult = result as UnauthorizedObjectResult;
            Assert.Equal(mockLoginResult, unauthorizedResult?.Value);
        }

        [Fact]
        public async Task Register_ValidRegisterInfo_ReturnsOk()
        {
            // Arrange            
            Register registerData = new()
            {
                Info = new Info
                {
                    FirstName = "John",
                    LastName = "Doe",
                    UserName = "johndoe",
                    Email = "john.doe@example.com"
                },
                Password = new CreatePassword
                {
                    Password = "Password123!",
                    ConfirmPassword = "Password123!"
                }
            };

            ResponseToken mockRegisterResult = new() { IsSucceed = true, Message = "Utilisateur créé avec succès" };
            _authServiceMock.Setup(service => service.RegisterAsync(registerData.Info, registerData.Password)).ReturnsAsync(mockRegisterResult);

            // Act
            IActionResult result = await _controller.Register(registerData);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            OkObjectResult? okResult = result as OkObjectResult;
            Assert.Equal(mockRegisterResult, okResult?.Value);
        }

        [Fact]
        public async Task Register_IncompleteRegisterInfo_ReturnsBadRequest()
        {
            // Arrange            
            Register registerData = new()
            {
                Info = new Info
                {
                    FirstName = "",
                    LastName = "Doe",
                    UserName = "johndoe",
                    Email = ""
                },
                Password = new CreatePassword
                {
                    Password = "Password123!",
                    ConfirmPassword = "Password123!"
                }
            };

            ResponseToken mockRegisterResult = new()
            {
                IsSucceed = false,
                Message = "Les informations de l'utilisateur ou le mot de passe sont manquants."
            };
            _authServiceMock.Setup(service => service.RegisterAsync(It.IsAny<Info>(), It.IsAny<CreatePassword>())).ReturnsAsync(mockRegisterResult);

            // Act
            IActionResult result = await _controller.Register(registerData);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            BadRequestObjectResult? badRequestResult = result as BadRequestObjectResult;
            Assert.Equal("Les informations de l'utilisateur ou le mot de passe sont manquants.", badRequestResult?.Value);
        }

        [Fact]
        public async Task UpdatePassword_ValidInput_ReturnsOk()
        {
            // Arrange       
            User? user = _contextMock?.Context?.Users?.FirstOrDefault(u => u.Id == "1");
            UpdatePassword updatePassword = new() { Username = user?.UserName, Password = new CreatePassword { Password = "Password123!", ConfirmPassword = "Password123!" } };

            if (user != null)
            {
                _userManagerMock.Setup(m => m.FindByNameAsync(user.UserName)).ReturnsAsync(user);
                _userManagerMock.Setup(m => m.RemovePasswordAsync(user)).ReturnsAsync(IdentityResult.Success);
                _userManagerMock.Setup(m => m.AddPasswordAsync(user, updatePassword.Password.Password)).ReturnsAsync(IdentityResult.Success);
            }

            // Act
            IActionResult result = await _controller.UpdatePassword(updatePassword);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdatePassword_InvalidUsername_ReturnsBadRequest()
        {
            // Arrange
            UpdatePassword updatePassword = new() { Username = null, Password = new CreatePassword { Password = "Password123!", ConfirmPassword = "Password123!" } };

            // Act
            IActionResult result = await _controller.UpdatePassword(updatePassword);

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Requête invalide", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdatePassword_RemovePasswordFailed_ReturnsBadRequest()
        {
            // Arrange
            User user = new();
            UpdatePassword updatePassword = new() { Username = "existing_user", Password = new CreatePassword { Password = "new_password", ConfirmPassword = "new_password" } };

            _userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.RemovePasswordAsync(user)).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Failed to remove password" }));
            _userManagerMock.Setup(m => m.AddPasswordAsync(user, updatePassword.Password.Password)).ReturnsAsync(IdentityResult.Success);

            // Act
            IActionResult result = await _controller.UpdatePassword(updatePassword);

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Échec de la suppression du mot de passe", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdatePassword_AddPasswordFailed_ReturnsBadRequest()
        {
            // Arrange
            User user = new(); 
            _userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.RemovePasswordAsync(user)).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(m => m.AddPasswordAsync(user, It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Failed to add password" }));

            UpdatePassword updatePassword = new() { Username = "existing_user", Password = new CreatePassword { Password = "new_password", ConfirmPassword = "new_password" } };

            // Act
            IActionResult result = await _controller.UpdatePassword(updatePassword);

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Échec de l'ajout du nouveau mot de passe", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdatePassword_NullPassword_ReturnsBadRequest()
        {
            // Arrange
            User user = new(){ UserName = "existinguser" };
            UpdatePassword updatePassword = new() { Username = "existinguser", Password = null };

            _userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.RemovePasswordAsync(user)).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(m => m.AddPasswordAsync(user, It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Failed to add password" }));

            // Act
            IActionResult result = await _controller.UpdatePassword(updatePassword);

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Échec de l'ajout du nouveau mot de passe", badRequestResult.Value);
        }

        [Fact]
        public async Task ResetPassword_ValidInput_ReturnsOk()
        {
            // Arrange
            User user = new();
            _userManagerMock.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.ResetPasswordAsync(user, It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            ResetPassword resetPassword = new() { Email = "user@example.com", Token = "reset_token", Password = new CreatePassword { Password = "Password123!", ConfirmPassword = "Password123!" } };

            // Act
            IActionResult result = await _controller.ResetPassword(resetPassword);

            // Assert
            Assert.IsType<OkResult>(result);
            string expectedToken = resetPassword.Token.Replace(" ", "+");
            _userManagerMock.Verify(m => m.ResetPasswordAsync(user, expectedToken, resetPassword.Password.Password), Times.Once);
        }

        [Fact]
        public async Task ResetPassword_ResetPasswordIsNullOrTokenIsNullOrEmpty_ReturnsBadRequest()
        {
            // Arrange            
            ResetPassword resetPasswordNull = new() { Email = null, Token = null, Password = new CreatePassword { Password = null, ConfirmPassword = null } };
            ResetPassword resetPasswordEmptyToken = new() { Email = "test@example.com", Token = "", Password = new CreatePassword { Password = "Password123!", ConfirmPassword = "Password123!" } };
            ResetPassword resetPasswordNullToken = new() { Email = "test@example.com", Token = null, Password = new CreatePassword { Password = "Password123!", ConfirmPassword = "Password123!" } };

            // Act
            IActionResult resultNull = await _controller.ResetPassword(resetPasswordNull);
            IActionResult resultEmptyToken = await _controller.ResetPassword(resetPasswordEmptyToken);
            IActionResult resultNullToken = await _controller.ResetPassword(resetPasswordNullToken);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultNull);
            Assert.IsType<BadRequestObjectResult>(resultEmptyToken);
            Assert.IsType<BadRequestObjectResult>(resultNullToken);
        }

        [Fact]
        public async Task ResetPassword_UserNotFound_ReturnsBadRequest()
        {
            // Arrange
            _userManagerMock.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null!);
            _userManagerMock.Setup(m => m.ResetPasswordAsync(null!, It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success!);

            ResetPassword resetPassword = new() { Email = "nonexistent@example.com", Token = "reset_token", Password = new CreatePassword { Password = "newpassword" } };

            // Act
            IActionResult result = await _controller.ResetPassword(resetPassword);

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Utilisateur non trouvé", badRequestResult.Value);
        }

        [Fact]
        public async Task ResetPassword_NullToken_ReturnsBadRequest()
        {
            // Arrange
            User user = new();
            _userManagerMock.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.ResetPasswordAsync(user, It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success!);

            ResetPassword resetPassword = new() { Email = "user@example.com", Token = null , Password = new CreatePassword { Password = "newpassword" } };

            // Act
            IActionResult result = await _controller.ResetPassword(resetPassword);

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Requête invalide", badRequestResult.Value);
        }

        [Fact]
        public async Task ForgotPassword_ValidEmail_ReturnsOk()
        {
            // Arrange
            string? email = "john@example.com";
            if(_contextMock.Context.Users != null)
            {
                User? user = await _contextMock.Context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user != null)
                    _userManagerMock.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
                _emailSenderMock.Setup(m => m.SendEmailAsync(It.IsAny<Message>()));
                ForgotPassword forgotPassword = new() { Email = "john@example.com" };

                // Act
                IActionResult result = await _controller.ForgotPassword(forgotPassword);

                // Assert
                OkResult okResult = Assert.IsType<OkResult>(result);
            }            
        }

        [Fact]
        public async Task ForgotPassword_InvalidEmail_ReturnsBadRequest()
        {
            // Arrange
            User user = null!; // Simuler qu'aucun utilisateur n'est trouvé avec l'email fourni
            _userManagerMock.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            ForgotPassword forgotPassword = new() { Email = "invalid_email@example.com" };

            // Act
            IActionResult result = await _controller.ForgotPassword(forgotPassword);

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Requête invalide", badRequestResult.Value);
        }
    }
}
