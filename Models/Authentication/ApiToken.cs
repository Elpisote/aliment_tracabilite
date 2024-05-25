namespace aliment_backend.Models.Authentication
{
    /// <summary>
    /// Représente un jeton d'API comprenant un jeton d'accès et un jeton de rafraîchissement.
    /// </summary>
    public class ApiToken
    {
        /// <summary>
        /// Obtient ou définit le jeton d'accès.
        /// </summary>
        public string? AccessToken { get; set; }

        /// <summary>
        /// Obtient ou définit le jeton de rafraîchissement.
        /// </summary>
        public string? RefreshToken { get; set; }
    }
}
