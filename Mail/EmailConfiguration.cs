namespace aliment_backend.Mail
{
    /// <summary>
    /// Représente la configuration de l'e-mail utilisée pour l'envoi des e-mails.
    /// </summary>
    public class EmailConfiguration
    {
        /// <summary>
        /// Obtient ou définit l'adresse e-mail de l'expéditeur.
        /// </summary>
        public string? FromEmail { get; set; }

        /// <summary>
        /// Obtient ou définit le nom de l'expéditeur.
        /// </summary>
        public string? FromName { get; set; }

        /// <summary>
        /// Obtient ou définit le serveur SMTP à utiliser pour l'envoi des e-mails.
        /// </summary>
        public string? SmtpServer { get; set; }

        /// <summary>
        /// Obtient ou définit le port utilisé pour la connexion au serveur SMTP.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Obtient ou définit le nom d'utilisateur utilisé pour l'authentification SMTP.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Obtient ou définit le mot de passe utilisé pour l'authentification SMTP.
        /// </summary>
        public string? Password { get; set; }
    }
}
