using aliment_backend.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace aliment_backend.Service
{
    /// <summary>
    /// Service pour la gestion des tokens JWT.
    /// </summary>
    public class TokenService : ITokenService
    {
        /// <summary>
        /// Génère un token d'accès JWT en fonction des revendications spécifiées.
        /// </summary>
        /// <param name="claims">Revendications pour le token JWT.</param>
        /// <returns>Token d'accès JWT.</returns>
        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            // Vérifier si la configuration est nulle avant d'accéder à la clé JWT
            string? jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JWT key est manquant dans la configuration.");
            }

            // Créer la clé secrète à partir de la clé de configuration
            SymmetricSecurityKey secretKey = new(Encoding.UTF8.GetBytes(jwtKey));

            // Créer les informations de signature pour le token JWT
            SigningCredentials signinCredentials = new(secretKey, SecurityAlgorithms.HmacSha256);

            // Créer les options pour le token JWT
            JwtSecurityToken tokeOptions = new(
                issuer: Environment.GetEnvironmentVariable("JWT_VALID_ISSUER"),
                audience: Environment.GetEnvironmentVariable("JWT_VALID_AUDIENCE"),
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: signinCredentials
            );

            // Générer le token JWT
            string tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }

        /// <summary>
        /// Génère un token de rafraîchissement aléatoire.
        /// </summary>
        /// <returns>Token de rafraîchissement.</returns>
        public string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// Obtient un ClaimsPrincipal à partir d'un token JWT expiré.
        /// </summary>
        /// <param name="token">Token JWT expiré.</param>
        /// <returns>ClaimsPrincipal.</returns>
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            string? jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
            if (string.IsNullOrEmpty(jwtKey))
            {
                // Gérer le cas où la clé JWT est absente ou nulle dans la configuration
                throw new InvalidOperationException("JWT key est manquant ou nul dans la configuration.");
            }

            // Paramètres de validation du token
            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ValidateLifetime = false
            };

            // Gestionnaire de token JWT
            JwtSecurityTokenHandler tokenHandler = new();
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                      
            return principal;
        }
    }
}
