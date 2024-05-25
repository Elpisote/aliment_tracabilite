using System.ComponentModel.DataAnnotations;

namespace aliment_backend.DTOs
{
    public class UserDTO
    {
        /// <summary>
        /// Nom de famille de l'utilisateur.
        /// </summary>
        [Required(ErrorMessage = "Le nom de famille est requis !")]
        [StringLength(30, ErrorMessage = "Doit contenir entre {2} and {1} caractères.", MinimumLength = 3)]
        public string? Lastname { get; set; }

        /// <summary>
        /// Prénom de l'utilisateur.
        /// </summary>
        [Required(ErrorMessage = "Le prénom est requis !")]
        [StringLength(30, ErrorMessage = "Doit contenir entre {2} and {1} caractères.", MinimumLength = 3)]
        public string? Firstname { get; set; }

        /// <summary>
        /// Pseudo de l'utilisateur.
        /// </summary>
        [Required(ErrorMessage = "Le pseudo est requis !")]
        [StringLength(30, ErrorMessage = "Doit contenir entre {2} and {1} caractères.", MinimumLength = 3)]
        public string? UserName { get; set; }

        /// <summary>
        /// Adresse e-mail de l'utilisateur.
        /// </summary>
        [Required(ErrorMessage = "L'email est requis !")]
        [Utils.Email(ErrorMessage = "L'adresse e-mail n'est pas valide.")]
        public string? Email { get; set; }

        /// <summary>
        /// Rôle de l'utilisateur.
        /// </summary>
        [Required]
        public string? Role { get; set; }          
    }
}
