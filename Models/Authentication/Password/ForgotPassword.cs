using System.ComponentModel.DataAnnotations;

namespace aliment_backend.Models.Authentication.Password
{
    /// <summary>
    /// Représente les données nécessaires pour demander la réinitialisation de mot de passe.
    /// </summary>
    public class ForgotPassword
    {
        /// <summary>
        /// Obtient ou définit l'adresse e-mail associée au compte utilisateur pour la réinitialisation du mot de passe.
        /// </summary>
        [EmailAddress(ErrorMessage = "Veuillez saisir une adresse e-mail valide.")]
        [Required(ErrorMessage = "L'adresse e-mail est requise.")]
        public string? Email { get; set; }
    }

}
