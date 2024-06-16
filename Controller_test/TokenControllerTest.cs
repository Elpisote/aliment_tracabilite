using aliment_backend.Controllers;
using aliment_backend.Entities;
using aliment_backend.Interfaces;
using aliment_backend.Models.Authentication;
using aliment_backend.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace unit_test.Controller_test
{
    public class TokenControllerTest
    {
        private readonly ApplicationDbContextMoq _contextMock = new();
        private readonly ITokenService _tokenService;
        private readonly TokenController _controller;
        private readonly string _token;

        public TokenControllerTest()
        {
            _tokenService = new TokenService();
            Environment.SetEnvironmentVariable("JWT_VALID_ISSUER", "issuer");
            Environment.SetEnvironmentVariable("JWT_VALID_AUDIENCE", "audience");
            Environment.SetEnvironmentVariable("JWT_KEY", "myverysecuresecretkeyforunittest123");
            _controller = new TokenController(_contextMock.Context, _tokenService);

            List<Claim> claims = new()
            {
                new Claim("Username", "evetaylor"),
                new Claim(ClaimTypes.Email, "eve@example.com"),
                new Claim(ClaimTypes.Role, "Admin")
            };
            _token = _tokenService.GenerateAccessToken(claims);
        }
        
        [Fact]
        public void Refresh_ApiTokenIsValid_ReturnsNewTokens()
        {
            // Arrange
            User? user = _contextMock?.Context?.Users?.SingleOrDefault(u => u.UserName == "evetaylor");

            if (user != null)
            {
                user.RefreshToken = "RefreshTokenValide";
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);
            }

            ApiToken apiToken = new()
            {
                AccessToken = _token,
                RefreshToken = "RefreshTokenValide"
            };

            Assert.NotNull(user);
            Environment.SetEnvironmentVariable("JWT_KEY", "myverysecuresecretkeyforunittest123");

            // Act
            OkObjectResult? result = _controller.Refresh(apiToken) as OkObjectResult;

            // Asset
            Assert.NotNull(result);
            Assert.Equal(200, result?.StatusCode);
            ResponseToken? responseToken = result?.Value as ResponseToken;
            Assert.NotNull(responseToken);
            Assert.True(responseToken?.IsSucceed);
            Assert.NotNull(responseToken?.AccessToken);
            Assert.NotNull(responseToken?.RefreshToken);
            Assert.Equal("Nouveau token généré", responseToken?.Message);
        }

        [Fact]
        public void Refresh_ApiTokenIsNull_ReturnsBadRequest()
        {
            // Arrange
            ApiToken apiToken = new()
            {
                AccessToken = null!,
                RefreshToken = "RefreshTokenValide"
            };
            // Act
            IActionResult result = _controller.Refresh(apiToken);

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Requête invalide", badRequestResult.Value);
        }

        [Fact]
        public void Refresh_UserIsNull_ReturnsBadRequest()
        {
            // Arrange
            ApiToken apiToken = new()
            {
                AccessToken = _token,
                RefreshToken = "RefreshTokenValide"
            };

            // Act
            IActionResult result = _controller.Refresh(apiToken);

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Requête invalide", badRequestResult.Value);
        }

        [Fact]
        public void Refresh_InvalidRefreshTokenExpiryTime_BadRequest()
        {
            // Arrange    
            User? user = _contextMock?.Context?.Users?.SingleOrDefault(u => u.UserName == "evetaylor");

            if (user != null)
            {
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(-1);
            }

            ApiToken apiToken = new()
            {
                AccessToken = _token,
                RefreshToken = "RefreshTokenValide"
            };

            // Act
            IActionResult? result = _controller.Refresh(apiToken);

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Requête invalide", badRequestResult.Value);
        }

        [Fact]
        public void Refresh_RefreshTokenMismatch_ReturnsBadRequest()
        {
            // Arrange
            User? user = _contextMock?.Context?.Users?.SingleOrDefault(u => u.UserName == "evetaylor");

            if (user != null)
            {
                user.RefreshToken = "RefreshTokenValide";
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);
            }

            ApiToken apiToken = new()
            {
                AccessToken = _token,
                RefreshToken = "AutreRefreshToken"
            };

            Assert.NotNull(user);
            Environment.SetEnvironmentVariable("JWT_KEY", "myverysecuresecretkeyforunittest123");

            // Act
            IActionResult result = _controller.Refresh(apiToken);

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Requête invalide", badRequestResult.Value);
        }

        [Fact]
        public void Refresh_WrongUsername_ReturnsBadRequest()
        {
            // Arrange            
            string refreshToken = "dummyRefreshToken";

            // Créer un ClaimsPrincipal avec une revendication Username nulle
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Email, "eve@example.com"),
                new Claim(ClaimTypes.Role, "Admin")
            };
            string token = _tokenService.GenerateAccessToken(claims);
            ApiToken apiToken = new ApiToken()
            {
                AccessToken = token,
                RefreshToken = refreshToken
            };

            // Act
            IActionResult result = _controller.Refresh(apiToken);

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Requête invalide", badRequestResult.Value);
        }
    }
}

