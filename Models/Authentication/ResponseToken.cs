namespace aliment_backend.Models.Authentication
{
    /// <summary>
    /// Représente la réponse renvoyée lors d'une opération de génération de jetons d'authentification.
    /// </summary>
    public class ResponseToken
    {
        /// <summary>
        /// Obtient ou définit une valeur indiquant si l'opération a réussi.
        /// </summary>
        public bool IsSucceed { get; set; }

        /// <summary>
        /// Obtient ou définit un message associé à la réponse.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Obtient ou définit le jeton d'accès généré.
        /// </summary>
        public string? AccessToken { get; set; }

        /// <summary>
        /// Obtient ou définit le jeton de rafraîchissement généré.
        /// </summary>
        public string? RefreshToken { get; set; }
    }
}
