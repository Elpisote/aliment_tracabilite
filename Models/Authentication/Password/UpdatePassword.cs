namespace aliment_backend.Models.Authentication.Password
{
    /// <summary>
    /// Représente les données nécessaires pour mettre à jour le mot de passe d'un utilisateur.
    /// </summary>
    public class UpdatePassword
    {
        /// <summary>
        /// Obtient ou définit les informations sur le nouveau mot de passe.
        /// </summary>
        public CreatePassword? Password { get; set; }

        /// <summary>
        /// Obtient ou définit le nom d'utilisateur de l'utilisateur dont le mot de passe doit être mis à jour.
        /// </summary>
        public string? Username { get; set; }
    }
}
