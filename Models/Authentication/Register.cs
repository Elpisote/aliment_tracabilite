using aliment_backend.Models.Authentication.Password;
using System.ComponentModel.DataAnnotations;

namespace aliment_backend.Models.Authentication
{
    /// <summary>
    /// Représente les informations nécessaires pour l'enregistrement d'un nouvel utilisateur.
    /// </summary>
    public class Register
    {
        /// <summary>
        /// Obtient ou définit les informations personnelles de l'utilisateur à enregistrer.
        /// </summary>
        [Required]
        public Info Info { get; set; } = new Info();

        /// <summary>
        /// Obtient ou définit le mot de passe pour le nouvel utilisateur.
        /// </summary>
        [Required]
        public CreatePassword Password { get; set; } = new CreatePassword();
    }
}
