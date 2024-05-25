namespace aliment_backend.Utils
{
    /// <summary>
    /// Classe statique contenant une méthode pour générer le corps d'un e-mail de réinitialisation de mot de passe.
    /// </summary>
    public static class EmailBody
    {
        /// <summary>
        /// Génère le corps HTML d'un e-mail de réinitialisation de mot de passe.
        /// </summary>
        /// <param name="email">L'adresse e-mail de l'utilisateur.</param>
        /// <param name="token">Le jeton de réinitialisation de mot de passe.</param>
        /// <returns>Une chaîne contenant le corps HTML de l'e-mail.</returns>
        public static string EmailStringBody(string email, string token)
        {
            return $@"
            <h1>Réinitialisation de votre mot de passe</h1><br />
            <p>Vous recevez cet e-mail car vous avez demandé à réinitialiser votre mot de passe.</p>
            <br />
            <p>Veuillez cliquer sur le lien ci-dessous pour créer un nouveau mot de passe :</p>
            <br />
            <a href=""http://localhost:4200/auth/resetPassword?email={email}&token={token}"" target=""_blank style=""background:black;padding:10px;border:none;color:white;border-radius:4px;display:block;margin:0 auto; width:50%;text-align:center;text-decoration:none"">Réinitialisation du mot de passe</a></br>
        ";
        }
    }
}
