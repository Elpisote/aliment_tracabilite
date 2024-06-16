using aliment_backend.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace unit_test.Service_test
{
    public class TokenServiceTest 
    {
        private readonly ApplicationDbContextMoq _contextMock = new();
        private readonly TokenService _tokenService;
        private readonly string jwtkey = "myverysecuresecretkeyforunittest123";

        public TokenServiceTest()
        {
            Environment.SetEnvironmentVariable("JWT_VALID_ISSUER", "issuer");
            Environment.SetEnvironmentVariable("JWT_VALID_AUDIENCE", "audience");
            _tokenService = new TokenService();
        }

        [Fact]
        public void GenerateAccessToken_WithValidConfiguration_ReturnsValidToken()
        {
            // Arrange
            Environment.SetEnvironmentVariable("JWT_KEY", jwtkey);

            List<Claim> claims = new()
            {
                new Claim("Username", "John Doe"),
                new Claim(ClaimTypes.Email, "john.doe@example.com"),
                new Claim(ClaimTypes.Role, "User")
            };

            // Act
            string token = _tokenService.GenerateAccessToken(claims);

            // Assert
            Assert.NotNull(token);

            if (token != null)
            {
                ClaimsPrincipal principal = _tokenService.GetPrincipalFromExpiredToken(token);
                List<Claim> tokenClaims = principal.Claims
                                                    .Where(c => claims.Any(cc => cc.Type == c.Type && cc.Value == c.Value))
                                                    .ToList();
                Assert.Equal(claims.Count, tokenClaims.Count);

                foreach (Claim claim in claims)
                {
                    Claim? matchingClaim = tokenClaims.FirstOrDefault(c => c.Type == claim.Type);
                    Assert.NotNull(matchingClaim);
                    Assert.Equal(claim.Value, matchingClaim?.Value);
                }
            }
        }

        [Fact]
        public void GenerateAccessToken_JwtKeyIsNull_ThrowsInvalidOperationException()
        {
            Environment.SetEnvironmentVariable("JWT_KEY", null);
          
            // Arrange
            List<Claim> claims = new()
            {
                new Claim("Username", "John Doe"),
                new Claim(ClaimTypes.Email, "john.doe@example.com"),
                new Claim(ClaimTypes.Role, "User")
            };
            Environment.SetEnvironmentVariable("JWT_KEY", null);
            string? test2 = Environment.GetEnvironmentVariable("JWT_KEY");
            // Act & Assert
            InvalidOperationException  exception = Assert.Throws<InvalidOperationException>(() => _tokenService.GenerateAccessToken(claims));
            Assert.Equal("JWT key est manquant dans la configuration.", exception.Message);
        }

        [Fact]
        public void GenerateAccessToken_JwtKeyIsEmpty_ThrowsInvalidOperationException()
        {
            // Arrange
            Environment.SetEnvironmentVariable("JWT_KEY", "");
            List<Claim> claims = new()
            {
                new Claim("Username", "John Doe"),
                new Claim(ClaimTypes.Email, "john.doe@example.com"),
                new Claim(ClaimTypes.Role, "User")
            };

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _tokenService.GenerateAccessToken(claims));
            Assert.Equal("JWT key est manquant dans la configuration.", exception.Message);
        }

        [Fact]
        public void GenerateRefreshToken_RefreshTokenValid_ReturnsNonEmptyString()
        {
            // Arrange
            Environment.SetEnvironmentVariable("JWT_KEY", jwtkey);

            // Act
            string refreshToken = _tokenService.GenerateRefreshToken();

            // Assert
            Assert.NotNull(refreshToken);
            Assert.NotEmpty(refreshToken);
        }       
    }
}
