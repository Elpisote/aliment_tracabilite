using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace aliment_backend.Entities
{
    /// <summary>
    /// Représente un utilisateur dans le système.
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// Prénom de l'utilisateur.
        /// </summary>
        [Required(ErrorMessage = "Le prénom est requis !")]
        [StringLength(30, ErrorMessage = "Doit contenir entre {2} and {1} caractères.", MinimumLength = 3)]
        public string? Firstname { get; set; }

        /// <summary>
        /// Nom de famille de l'utilisateur.
        /// </summary>
        [Required(ErrorMessage = "Le nom de famille est requis !")]
        [StringLength(30, ErrorMessage = "Doit contenir entre {2} and {1} caractères.", MinimumLength = 3)]
        public string? Lastname { get; set; }

        /// <summary>
        /// Jeton de rafraîchissement utilisé pour l'authentification de l'utilisateur (ignoré lors de la sérialisation JSON).
        /// </summary>
        [JsonIgnore]
        public string? RefreshToken { get; set; }

        /// <summary>
        /// Date d'expiration du jeton de rafraîchissement.
        /// </summary>
        [JsonIgnore]
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}

