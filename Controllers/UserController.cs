using aliment_backend.DTOs;
using aliment_backend.Entities;
using aliment_backend.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace aliment_backend.Controllers
{
    // Ce contrôleur gère les opérations liées aux utilisateurs
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        // Constructeur du contrôleur avec les dépendances nécessaires
        public UserController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)           
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;                  
        }

        // Méthode pour obtenir tous les utilisateurs 
        [Authorize(Roles = "Admin, User")]
        [HttpGet("api/[controller]s")]
        public async Task<IActionResult> GetAllAsync()
        {      
            // Récupérer tous les utilisateurs
            List<User> users = (List<User>)await _unitOfWork.Users.GetAllAsync();

            // Créer une liste pour stocker les données des utilisateurs avec leurs rôles
            List<object> usersWithRoles = new();

                // Parcourir tous les utilisateurs pour obtenir leurs rôles
                foreach (User user in users)
                {
                    // Récupérer les rôles de l'utilisateur
                    IList<string> roles = await _userManager.GetRolesAsync(user);

                    // Ajouter les données de l'utilisateur avec ses rôles à la liste
                    usersWithRoles.Add(new
                    {
                        user.Id,
                        user.Firstname,
                        user.Lastname,
                        user.Email,
                        user.UserName,
                        Role = string.Join(", ", roles)
                    });
                }
                // Retourner la liste des utilisateurs avec leurs rôles
            return Ok(usersWithRoles);            
        }

        // Méthode pour obtenir un utilisateur par son nom d'utilisateur
        [Authorize(Roles = "Admin, User")]
        [HttpGet("api/[controller]/{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {           
            // Vérifier si l'ID ou le nom d'utilisateur est null
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("L'ID est nul ou vide.");
            }

            User user = await _userManager.FindByIdAsync(id);

            // Vérifier si l'utilisateur est trouvé
            if (user == null)
                return NotFound($"L'utilisateur avec l'identifiant {id} non trouvé.");

            // Récupérer les rôles de l'utilisateur
            IList<string> roles = await _userManager.GetRolesAsync(user);

            // Retourner les données de l'utilisateur avec ses rôles
            var userWithRoles = new
            {
                user.Id,
                user.Firstname,
                user.Lastname,
                user.Email,
                user.UserName,
                Role = string.Join(", ", roles)
            };
            return Ok(userWithRoles);
            
        }

        // Méthode pour la suppression des utilisateurs
        [Authorize(Roles = "Admin")]
        [HttpDelete("api/[controller]/{id}")]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            // Recherche de l'utilisateur par son nom d'utilisateur
            User user = await _userManager.FindByIdAsync(id);

            // Vérifier si l'utilisateur existe
            if (user == null)
            {
                return NotFound($"L'utilisateur avec l'identifiant {id} non trouvé.");
            }

            // Supprimer les claims de l'utilisateur
            IList<Claim> allClaims = await _userManager.GetClaimsAsync(user);
            await _userManager.RemoveClaimsAsync(user, (IEnumerable<Claim>)allClaims);

            // Supprimer le rôle de l'utilisateur
            var role = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, role);

            // Supprimer l'utilisateur
            await _userManager.DeleteAsync(user);

            // Retourner une valeur booléenne indiquant le succès de la suppression
            return true;
        }

        //Méthode pour mettre à jour les données de l'utilisateur
        [Authorize(Roles = "Admin, User")]
        [HttpPut("api/[controller]/{id}")]
        public async Task<IActionResult> UpdateUserAsync(string id, UserDTO userDTO)
        {
            // Recherche de l'utilisateur par son nom d'utilisateur
            User user = await _userManager.FindByIdAsync(id);

            // Vérifier si l'utilisateur existe
            if (user == null)
            {
                return NotFound($"L'utilisateur avec l'identifiant {id} non trouvé.");
            }

            // Récupérer les claims de l'utilisateur
            IList<Claim> claims = await _userManager.GetClaimsAsync(user);

            // Changer le nom d'utilisateur si différent
            if (userDTO.UserName != null && user.UserName != userDTO.UserName)
            {
                Claim? oldUsername = claims.FirstOrDefault( c => c.Type == "Username");
                Claim newUsername = new ("Username", userDTO.UserName);
                await _userManager.ReplaceClaimAsync(user, oldUsername, newUsername);
            }

            // Changer l'adresse e-mail si différente
            if (userDTO.Email != null && user.Email != userDTO.Email)
            {
                Claim? oldEmail = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                Claim newEmail = new (ClaimTypes.Email, userDTO.Email);
                await _userManager.ReplaceClaimAsync(user, oldEmail, newEmail);
            }
           
            // Trouver la revendication du rôle actuel de l'utilisateur
            Claim? oldRole = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

            // Changer le role si différent
            if (userDTO.Role != null && userDTO.Role != oldRole?.Value)
            {
                // Supprimer tous les rôles actuels de l'utilisateur
                string[] allRoles = _userManager.GetRolesAsync(user).Result.ToArray();
                await _userManager.RemoveFromRolesAsync(user, allRoles);

                // Ajouter le nouveau rôle à l'utilisateur
                await _userManager.AddToRoleAsync(user, userDTO.Role);

                // Créer une nouvelle revendication avec le nouveau rôle
                Claim newRole = new(ClaimTypes.Role, userDTO.Role);

                // Remplacer la revendication du rôle dans les informations de l'utilisateur
                await _userManager.ReplaceClaimAsync(user, oldRole, newRole);
            }

            // Mettre à jour les informations de l'utilisateur en fonction du DTO
            _mapper.Map(userDTO, user);
            await _userManager.UpdateAsync(user);
            // Mettre à jour le rôle dans le DTO avec les nouveaux rôles de l'utilisateur
            userDTO.Role = string.Join(", ", _userManager.GetRolesAsync(user).Result.ToArray());

            return Ok(userDTO);
        }
    }  
}    


