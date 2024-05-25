namespace aliment_backend.Models.Authentication.Password
{
    /// <summary>
    /// Représente les informations nécessaires pour réinitialiser le mot de passe d'un utilisateur.
    /// </summary>
    public class ResetPassword
    {
        /// <summary>
        /// Obtient ou définit le nouveau mot de passe pour l'utilisateur.
        /// </summary>
        public CreatePassword? Password { get; set; }

        /// <summary>
        /// Obtient ou définit l'adresse e-mail de l'utilisateur dont le mot de passe doit être réinitialisé.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Obtient ou définit le jeton de réinitialisation du mot de passe.
        /// </summary>
        public string? Token { get; set; }
    }
}
