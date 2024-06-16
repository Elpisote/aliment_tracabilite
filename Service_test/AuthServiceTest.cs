using aliment_backend.Entities;
using aliment_backend.Interfaces;
using aliment_backend.Models.Authentication;
using aliment_backend.Models.Authentication.Password;
using aliment_backend.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Security.Claims;

namespace unit_test.Service_test
{
    public class AuthServiceTest
    {
        private readonly ApplicationDbContextMoq _contextMock = new();
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<ITokenService> _tokenServiceMock;

        public AuthServiceTest()
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

            _tokenServiceMock = new Mock<ITokenService>();
        }

        [Fact]
        public async Task RegisterAsync_NewUser_ReturnsResponseToken()
        {
            // Arrange
            _userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((string username) => null!);
            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(m => m.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(m => m.AddClaimAsync(It.IsAny<User>(), It.IsAny<Claim>()))
                .ReturnsAsync(IdentityResult.Success);

            AuthService authService = new(_userManagerMock.Object, _contextMock.Context, _tokenServiceMock.Object);

            Info info = new() { Email = "test@example.com", FirstName = "John", LastName = "Doe", UserName = "johndoe" };
            CreatePassword password = new() { Password = "Password123!", ConfirmPassword = "Password123!" };

            // Act
            ResponseToken result = await authService.RegisterAsync(info, password);

            // Assert
            Assert.True(result.IsSucceed);
            Assert.Equal("Utilisateur créé avec succès", result.Message);
        }

        [Fact]
        public async Task RegisterAsync_InfoIsNull_ReturnErrorResponse()
        {
            // Arrange
            AuthService authService = new(_userManagerMock.Object, _contextMock.Context, _tokenServiceMock.Object);
            CreatePassword password = new() { Password = "Password123!", ConfirmPassword = "Password123!" };
            Info info = null!;

            // Act
            ResponseToken result = await authService.RegisterAsync(info, password);

            // Assert
            Assert.False(result.IsSucceed);
            Assert.Equal("Les informations de l'utilisateur ou le mot de passe sont manquants.", result.Message);
        }

        [Fact]
        public async Task RegisterAsync_PasswordIsNull_ReturnErrorResponse()
        {
            // Arrange
            Info info = new() { Email = "test@example.com", FirstName = "John", LastName = "Doe", UserName = "johndoe" };
            AuthService authService = new(_userManagerMock.Object, _contextMock.Context, _tokenServiceMock.Object);

            // Act
            ResponseToken result = await authService.RegisterAsync(info, new CreatePassword());

            // Assert
            Assert.False(result.IsSucceed);
            Assert.Equal("Les informations de l'utilisateur ou le mot de passe sont manquants.", result.Message);
        }

        [Fact]
        public async Task RegisterAsync_InvalidPassword_ReturnsErrorResponse()
        {
            // Arrange
            _userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((string username) => null!);

            AuthService authService = new(_userManagerMock.Object, _contextMock.Context, _tokenServiceMock.Object);

            Info info = new() { Email = "test@example.com", FirstName = "John", LastName = "Doe", UserName = "johndoe" };
            CreatePassword password = new() { Password = "weak", ConfirmPassword = "weak" };

            // Act
            ResponseToken result = await authService.RegisterAsync(info, password);

            // Assert
            Assert.False(result.IsSucceed);
            Assert.Equal("Le mot de passe est invalide. Il doit comporter au moins 8 caractères, inclure une majuscule, une minuscule et un caractère spécial.", result.Message);
        }

        [Fact]
        public async Task RegisterAsync_usernameExist_ReturnsResponseToken()
        {
            // Arrange
            _userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((string username) => new User());

            AuthService authService = new(_userManagerMock.Object, _contextMock.Context, _tokenServiceMock.Object);

            Info info = new() { Email = "test@example.com", FirstName = "John", LastName = "Doe", UserName = "johndoe" };
            CreatePassword password = new() { Password = "Password123!", ConfirmPassword = "Password123!" };

            // Act
            ResponseToken result = await authService.RegisterAsync(info, password);

            // Assert
            Assert.False(result.IsSucceed);
            Assert.Equal("Cet identifiant exsite déjà", result.Message);
        }

        [Fact]
        public async Task RegisterAsync_UserCreationFails_ReturnErrorResponse()
        {
            // Arrange
            Info info = new() { Email = "test@example.com", FirstName = "John", LastName = "Doe", UserName = "johndoe" };
            CreatePassword password = new() { Password = "Password123!", ConfirmPassword = "Password123!" };
            AuthService authService = new(_userManagerMock.Object, _contextMock.Context, _tokenServiceMock.Object);

            _userManagerMock.Setup(um => um.FindByNameAsync(info.UserName)).ReturnsAsync((User)null!); // Simuler qu'aucun utilisateur avec le même nom n'existe
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), password.Password)).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "La création de l'utilisateur a échoué." }));

            // Act
            ResponseToken result = await authService.RegisterAsync(info, password);

            // Assert
            Assert.False(result.IsSucceed);
            Assert.Equal("La création du nouvel utilisateur a échoué parce que : La création de l'utilisateur a échoué.", result.Message);
        }

        [Fact]
        public async Task RegisterAsync_EmailIsNull_ReturnErrorResponse()
        {
            // Arrange
            AuthService authService = new(_userManagerMock.Object, _contextMock.Context, _tokenServiceMock.Object);
            CreatePassword password = new() { Password = "Password123!", ConfirmPassword = "Password123!" };
            Info info = new() { Email = "test@example.com", FirstName = "John", LastName = "Doe", UserName = "johndoe" };

            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Le champ Email est requis." }));

            // Act
            ResponseToken result = await authService.RegisterAsync(info, password);

            // Assert
            Assert.False(result.IsSucceed);
            Assert.Equal("La création du nouvel utilisateur a échoué parce que : Le champ Email est requis.", result.Message);
        }

        [Fact]
        public async Task LoginAsync_ValidLogin_ReturnsResponseTokenWithTokens()
        {
            // Arrange: Création des données de test
            Login login = new() { Email = "john@example.com", Password = "Password123!" };
            User? user = _contextMock.Context.Users?.FirstOrDefault(u => u.Email == login.Email);
            Assert.NotNull(user);

            if (user != null)
            {
                List<Claim> authClaims = new() { new Claim("Username", user.UserName) };
                _userManagerMock.Setup(m => m.FindByEmailAsync(login.Email))
                    .ReturnsAsync(user);
                _userManagerMock.Setup(m => m.CheckPasswordAsync(user, login.Password))
                    .ReturnsAsync(true);
                _userManagerMock.Setup(m => m.GetClaimsAsync(user))
                    .ReturnsAsync(authClaims);

                string accessToken = "accessToken";
                string refreshToken = "refreshToken";

                _tokenServiceMock.Setup(m => m.GenerateAccessToken(authClaims))
                    .Returns(accessToken);
                _tokenServiceMock.Setup(m => m.GenerateRefreshToken())
                    .Returns(refreshToken);

                AuthService authService = new(_userManagerMock.Object, _contextMock.Context, _tokenServiceMock.Object);

                // Act
                ResponseToken result = await authService.LoginAsync(login);

                // Assert
                Assert.Equal(accessToken, result.AccessToken);
                Assert.Equal(refreshToken, result.RefreshToken);
            }            
        }

        [Fact]
        public async Task LoginAsync_UserNull_ReturnsIsSucceedFalse()
        {
            // Arrange
            Login login = new() { Email = "nothing@example.com", Password = "Password123!" };
            _userManagerMock.Setup(um => um.FindByEmailAsync(login.Email)).ReturnsAsync((User)null!);
            AuthService authService = new(_userManagerMock.Object, _contextMock.Context, _tokenServiceMock.Object);

            // Act
            ResponseToken result = await authService.LoginAsync(login);

            // Assert: Vérifier que le résultat est conforme aux attentes
            Assert.False(result.IsSucceed);
            Assert.Equal("Email ou mot de passe incorrect", result.Message);
            _userManagerMock.Verify(um => um.FindByEmailAsync(login.Email), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_WrongPassword_ReturnsIsSucceedFalse()
        {
            // Arrange
            Login login = new() { Email = "john@example.com", Password = "WrongPassword123!" };
            User? user = _contextMock.Context.Users?.FirstOrDefault(u => u.Email == login.Email);
            Assert.NotNull(user);

            if (user != null)
            {               
                _userManagerMock.Setup(m => m.FindByEmailAsync(login.Email))
                    .ReturnsAsync(user);
                _userManagerMock.Setup(m => m.CheckPasswordAsync(user, login.Password))
                    .ReturnsAsync(false);

                AuthService authService = new(_userManagerMock.Object, _contextMock.Context, _tokenServiceMock.Object);

                // Act
                ResponseToken result = await authService.LoginAsync(login);

                // Assert
                Assert.False(result.IsSucceed);
                Assert.Equal("Email ou mot de passe incorrect", result.Message);

                _userManagerMock.Verify(m => m.FindByEmailAsync(login.Email), Times.Once);
                _userManagerMock.Verify(m => m.CheckPasswordAsync(user, login.Password), Times.Once);
            }
        }
    }
}
