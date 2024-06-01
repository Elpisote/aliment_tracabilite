using aliment_backend.Entities;
using aliment_backend.Interfaces;
using aliment_backend.Models.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace aliment_backend.Controllers
{
    // Ce contrôleur gère les opérations liées aux jetons d'authentification.
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;

        // Constructeur du contrôleur avec le contexte de la base de données et le service de jetons en tant que dépendances
        public TokenController(ApplicationDbContext userContext, ITokenService tokenService)
        {
            _context = userContext;
            _tokenService = tokenService;
          
        }

        // Méthode pour rafraîchir le jeton d'authentification
        [HttpPost]
        [Route("Refresh")]
        public IActionResult Refresh(ApiToken apiToken)
        {
            // Vérifier si l'objet ApiToken est null
            if (apiToken.AccessToken is null)
                return BadRequest("Requête invalide");

            // Récupérer les jetons d'accès et de rafraîchissement du client
            string? accessToken = apiToken.AccessToken;
            string? refreshToken = apiToken.RefreshToken;

         
            // Récupérer le principal des revendications du jeton d'accès expiré
            ClaimsPrincipal principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            string? username = principal.Claims.FirstOrDefault(c => c.Type== "Username")?.Value;

            // Rechercher l'utilisateur dans la base de données
            User? user = _context?.Users?.SingleOrDefault(u => u.UserName == username);

            // Vérifier si l'utilisateur est trouvé et si le jeton de rafraîchissement est valide
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Requête invalide");
            }
            else
            {
                // Générer de nouveaux jetons d'accès et de rafraîchissement
                string newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
                string newRefreshToken = _tokenService.GenerateRefreshToken();

                // Mettre à jour le jeton de rafraîchissement de l'utilisateur dans la base de données
                user.RefreshToken = newRefreshToken;
                _context?.SaveChanges();

                // Retourner une réponse OK avec les nouveaux jetons générés
                return Ok(new ResponseToken()
                {
                    IsSucceed = true,
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    Message = "Nouveau token généré"
                });
            }            
        }
    }
}
