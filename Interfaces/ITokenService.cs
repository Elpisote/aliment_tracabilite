using System.Security.Claims;

namespace aliment_backend.Interfaces
{
    /// <summary>
    /// Interface pour la gestion des jetons d'authentification.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Génère un jeton d'accès en utilisant les revendications spécifiées.
        /// </summary>
        /// <param name="claims">Les revendications à inclure dans le jeton d'accès.</param>
        /// <returns>Le jeton d'accès généré.</returns>
        string GenerateAccessToken(IEnumerable<Claim> claims);

        /// <summary>
        /// Génère un nouveau jeton de rafraîchissement.
        /// </summary>
        /// <returns>Le nouveau jeton de rafraîchissement généré.</returns>
        string GenerateRefreshToken();

        /// <summary>
        /// Récupère un principal de revendications à partir d'un jeton expiré.
        /// </summary>
        /// <param name="token">Le jeton expiré à partir duquel extraire les revendications.</param>
        /// <returns>Le principal de revendications extrait du jeton expiré.</returns>
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
