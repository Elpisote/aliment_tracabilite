using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace aliment_backend.Controllers
{
    // Ce contrôleur gère les opérations liées aux rôles d'utilisateur.
    [Authorize(Roles = "Admin")] 
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        // Constructeur du contrôleur avec le gestionnaire de rôles en tant que dépendance
        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        // Méthode pour récupérer tous les rôles d'utilisateur
        [HttpGet("api/[controller]s")]
        public IActionResult GetAllUser()
        {
            // Récupérer tous les rôles et les convertir en une liste pour la réponse
            return Ok(_roleManager.Roles.ToList());
        }
    }
}
