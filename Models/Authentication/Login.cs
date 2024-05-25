using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace aliment_backend.Models.Authentication
{
    /// <summary>
    /// Représente les informations nécessaires pour l'authentification d'un utilisateur.
    /// </summary>
    public class Login
    {
        /// <summary>
        /// Obtient ou définit l'adresse e-mail de l'utilisateur.
        /// </summary>
        [Required(ErrorMessage = "L'adresse e-mail est obligatoire.")]
        [EmailAddress(ErrorMessage = "Adresse e-mail invalide.")]
        public string? Email { get; set; }

        /// <summary>
        /// Obtient ou définit le mot de passe de l'utilisateur.
        /// </summary>
        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        public string? Password { get; set; }
    }
}
