using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using aliment_backend.Entities;
using aliment_backend.Interfaces;
using aliment_backend.Mail;
using aliment_backend.Models.Authentication;
using aliment_backend.Models.Authentication.Password;
using aliment_backend.Utils;

namespace aliment_backend.Controllers
{
    // Contrôleur pour gérer les opérations d'authentification des utilisateurs
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService; // Service pour l'authentification personnalisée
        private readonly UserManager<User> _userManager; // Gestionnaire d'utilisateurs ASP.NET Core Identity
        private readonly IEmailSender _emailSender; // Service pour l'envoi d'e-mails
    
        // Constructeur pour injecter les dépendances requises
        public AuthController(IAuthService authService, UserManager<User> userManager, IEmailSender emailSender)
        {
            _authService = authService;
            _userManager = userManager;
            _emailSender = emailSender;        
        }
        
        // Authentifie un utilisateur avec les informations de connexion fournies
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {            
            // Authentification de l'utilisateur et génération du token d'accès
            ResponseToken loginResult = await _authService.LoginAsync(login);

            // Vérification du résultat de l'authentification
            if (loginResult.IsSucceed)
            {
                return Ok(loginResult);
            }
            else
            {
                return Unauthorized(loginResult);
            }           
        }

        // Enregistre un nouvel utilisateur avec les informations d'enregistrement fournies
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(Register register)
        {
            // Enregistrement du nouvel utilisateur et génération du token d'accès
            ResponseToken registerResult = await _authService.RegisterAsync(register.Info, register.Password);

            // Vérification du résultat de l'enregistrement
            if (registerResult.IsSucceed)
            {
                return Ok(registerResult);
            }
            else
            {
                return BadRequest(registerResult.Message); // Retourner le message du ResponseToken
            }
        }

        // Met à jour le mot de passe de l'utilisateur
        [HttpPost("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(UpdatePassword updatePassword)
        {
            // Recherche de l'utilisateur par son nom d'utilisateur
            User user = await _userManager.FindByNameAsync(updatePassword.Username);
            if (user == null)
            {
                return BadRequest("Requête invalide");
            }
            else
            {
                // Suppression de l'ancien mot de passe de l'utilisateur
                IdentityResult removeResult = await _userManager.RemovePasswordAsync(user);
                if (!removeResult.Succeeded)
                {
                    return BadRequest("Échec de la suppression du mot de passe");
                }
                // Ajout du nouveau mot de passe à l'utilisateur
                IdentityResult addResult =  await _userManager.AddPasswordAsync(user, updatePassword?.Password?.Password);
                if (!addResult.Succeeded)
                {
                    return BadRequest("Échec de l'ajout du nouveau mot de passe");
                }
                return Ok();
            }            
        }

        // Réinitialise le mot de passe de l'utilisateur
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            // Vérification si resetPassword est null ou si resetPassword.Token est null ou vide
            if (string.IsNullOrEmpty(resetPassword.Email) || string.IsNullOrEmpty(resetPassword.Token))               
            {
                return BadRequest("Requête invalide");
            } 
            else
            {
                // Recherche de l'utilisateur par son adresse e-mail
                User user = await _userManager.FindByEmailAsync(resetPassword.Email);
                if (user == null)
                {
                    return BadRequest("Utilisateur non trouvé");
                }
                else
                {
                    // Réinitialisation du mot de passe de l'utilisateur à l'aide du token de réinitialisation
                    string? token = resetPassword?.Token?.Replace(" ", "+");  
                    await _userManager.ResetPasswordAsync(user, token, resetPassword?.Password?.Password);
                    return Ok();        
                }                  
            }            
        }

        // Envoie un e-mail de réinitialisation de mot de passe à l'utilisateur
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPassword forgotPassword)
        {            
            // Recherche de l'utilisateur par son adresse e-mail
            User user = await _userManager.FindByEmailAsync(forgotPassword.Email);

            if (user == null)
            {
                return BadRequest("Requête invalide");

            }
            else
            {
                // Génération d'un token de réinitialisation de mot de passe
                string token = await _userManager.GeneratePasswordResetTokenAsync(user);

                Message message = new(new MailboxAddress(user.Lastname, user.Email), "Mot de passe oublié", EmailBody.EmailStringBody(user.Email, token));
                _emailSender.SendEmailAsync(message);
                return Ok();
            }            
        }
    }
}
