using Microsoft.AspNetCore.Identity;
using aliment_backend.Entities;
using aliment_backend.Interfaces;
using aliment_backend.Models.Authentication;
using aliment_backend.Utils;
using System.Security.Claims;
using aliment_backend.Models.Authentication.Password;

namespace aliment_backend.Service
{
    /// <summary>
    /// Service d'authentification permettant de gérer les opérations de connexion et d'inscription des utilisateurs.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;        
        protected readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="AuthService"/>.
        /// </summary>
        /// <param name="userManager">Gestionnaire des utilisateurs.</param>
        /// <param name="context">Contexte de la base de données de l'application.</param>
        /// <param name="tokenService">Service de gestion des jetons d'authentification.</param>
        public AuthService(UserManager<User> userManager, ApplicationDbContext context, 
            ITokenService tokenService)
        {
            _userManager = userManager;           
            _context = context;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Authentifie un utilisateur en vérifiant les informations de connexion fournies.
        /// </summary>
        /// <param name="login">Les informations de connexion de l'utilisateur.</param>
        /// <returns>Un objet <see cref="ResponseToken"/> représentant le résultat de l'opération d'authentification.</returns>
        public async Task<ResponseToken> LoginAsync(Login login)
        {
            // Vérifier si l'utilisateur existe et si le mot de passe est correct 
            User user = await _userManager.FindByEmailAsync(login.Email);            
            bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, login.Password);

            // Si l'utilisateur n'existe pas ou si le mot de passe est incorrect, retourner un message d'erreur
            if (user is null || !isPasswordCorrect)
                return new ResponseToken()
                {
                    IsSucceed = false,
                    Message = "Email ou mot de passe incorrect"
                };

            // Récupérer les claims d'authentification de l'utilisateur
            IList<Claim> authClaims = await _userManager.GetClaimsAsync(user);

            // Générer un jeton d'accès et un jeton de rafraîchissement
            string accessToken = _tokenService.GenerateAccessToken(authClaims);
            string refreshToken = _tokenService.GenerateRefreshToken();

            // Mettre à jour le jeton de rafraîchissement de l'utilisateur dans la base de données
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);            
            _context.SaveChanges();

            // Retourner les jetons d'authentification générés
            return new ResponseToken()
            {
                IsSucceed = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
        
        /// <summary>
        /// Inscrit un nouvel utilisateur avec les informations fournies.
        /// </summary>
        /// <param name="info">Les informations personnelles de l'utilisateur à inscrire.</param>
        /// <param name="password">Les informations de mot de passe de l'utilisateur à inscrire.</param>
        /// <returns>Un objet <see cref="ResponseToken"/> représentant le résultat de l'opération d'inscription.</returns>
        public async Task<ResponseToken> RegisterAsync(Info info, CreatePassword password)
        {
            // Vérifier si info ou password sont null
            if (info == null || password.Password == null)
            {
                return new ResponseToken()
                {
                    IsSucceed = false,
                    Message = "Les informations de l'utilisateur ou le mot de passe sont manquants."
                };
            }

            // Vérifier si un utilisateur avec le même identifiant existe déjà
            User isExistsUser = await _userManager.FindByNameAsync(info.UserName);

            // Si un utilisateur avec le même identifiant existe déjà, retourner un message d'erreur
            if (isExistsUser != null)
                return new ResponseToken()
                {
                    IsSucceed = false,
                    Message = "Cet identifiant exsite déjà"
                };

            // Vérifier si le mot de passe est invalide
            if (!IsPasswordValid(password.Password))
            {
                return new ResponseToken()
                {
                    IsSucceed = false,
                    Message = "Le mot de passe est invalide. Il doit comporter au moins 8 caractères, inclure une majuscule, une minuscule et un caractère spécial."
                };
            }

            // Créer un nouvel utilisateur avec les informations fournies
            User newUser = new()
            {
                Firstname = info.FirstName,
                Lastname = info.LastName,
                Email = info.Email,
                UserName = info.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            // Créer l'utilisateur dans la base de données avec le mot de passe fourni
            IdentityResult createUserResult = await _userManager.CreateAsync(newUser, password.Password);

            // Si la création de l'utilisateur a échoué, retourner un message d'erreur
            if (!createUserResult.Succeeded)
            {
                var errorString = "La création du nouvel utilisateur a échoué parce que : ";
                foreach (var error in createUserResult.Errors)
                {
                    errorString +=  error.Description;
                }
                return new ResponseToken()
                {
                    IsSucceed = false,
                    Message = errorString
                };
            }

            // Ajouter le nouvel utilisateur au rôle par défaut
            await _userManager.AddToRoleAsync(newUser, StaticUserRole.USER);

            // Ajouter les claims d'authentification pour le nouvel utilisateur
            if (newUser.UserName != null && newUser.Email != null)
            {
                _ = await _userManager.AddClaimAsync(newUser, new Claim("Username", newUser.UserName));
                _ = await _userManager.AddClaimAsync(newUser, new Claim(ClaimTypes.Email, newUser.Email));
                _ = await _userManager.AddClaimAsync(newUser, new Claim(ClaimTypes.Role, StaticUserRole.USER));
            }

            // Retourner une réponse indiquant que l'utilisateur a été créé avec succès
            return new ResponseToken()
            {
                IsSucceed = true,
                Message = "Utilisateur créé avec succès"
            };
        }

        // Fonction pour valider le format du mot de passe
        private static bool IsPasswordValid(string password)
        {       
            // vérifier si le mot de passe a au moins 8 caractères, une majuscule, une minuscule et un caractère spécial
            return password.Length >= 8 &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsLower) &&
                   password.Any(char.IsDigit) &&
                   password.Any(c => !char.IsLetterOrDigit(c));
        }
    }       
}
