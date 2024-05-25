using System.ComponentModel.DataAnnotations;

namespace aliment_backend.Models.Authentication
{
    /// <summary>
    /// Représente les informations nécessaires pour l'inscription d'un nouvel utilisateur.
    /// </summary>
    public class Info
    {
        /// <summary>
        /// Obtient ou définit le prénom de l'utilisateur.
        /// </summary>
        [Required(ErrorMessage = "Le prénom est requis !")]
        [StringLength(30, ErrorMessage = "Doit contenir entre {2} et {1} caractères.", MinimumLength = 3)]
        [RegularExpression(@"^[\'\s- \p{Ll}\p{Lu}]+$", ErrorMessage = "Les caractères ne sont pas autorisés.")]
        public string? FirstName { get; set; }

        /// <summary>
        /// Obtient ou définit le nom de famille de l'utilisateur.
        /// </summary>
        [Required(ErrorMessage = "Le nom de famille est requis !")]
        [StringLength(30, ErrorMessage = "Doit contenir entre {2} et {1} caractères.", MinimumLength = 3)]
        [RegularExpression(@"^[\'\s- \p{Ll}\p{Lu}]+$", ErrorMessage = "Les caractères ne sont pas autorisés.")]
        public string? LastName { get; set; }

        /// <summary>
        /// Obtient ou définit le nom d'utilisateur de l'utilisateur.
        /// </summary>
        [Required(ErrorMessage = "Le nom d'utilisateur est requis !")]
        [StringLength(60, ErrorMessage = "Doit contenir entre {2} et {1} caractères.", MinimumLength = 3)]
        public string? UserName { get; set; }

        /// <summary>
        /// Obtient ou définit l'adresse e-mail de l'utilisateur.
        /// </summary>
        [EmailAddress(ErrorMessage = "Veuillez saisir une adresse e-mail valide.")]
        [Required(ErrorMessage = "L'adresse e-mail est requise.")]
        public string? Email { get; set; }
    }
}
