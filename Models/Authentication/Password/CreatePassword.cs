using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace aliment_backend.Models.Authentication.Password
{
    /// <summary>
    /// Représente les données nécessaires pour créer un mot de passe utilisateur.
    /// </summary>
    public class CreatePassword
    {
        /// <summary>
        /// Obtient ou définit le mot de passe de l'utilisateur.
        /// </summary>
        [Required(ErrorMessage = "Le mot de passe est requis.")]
        [StringLength(50, ErrorMessage = "Le {0} doit comporter au moins {2} et au maximum {1} caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        /// <summary>
        /// Obtient ou définit la confirmation du mot de passe de l'utilisateur.
        /// </summary>
        [NotMapped]
        [Required(ErrorMessage ="La confirmation du mot de passe est requise")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Le mot de passe et sa confirmation ne correspondent pas.")]
        public string? ConfirmPassword { get; set; }
    }
}
