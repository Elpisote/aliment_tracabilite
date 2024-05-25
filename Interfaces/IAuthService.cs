using aliment_backend.Models.Authentication;
using aliment_backend.Models.Authentication.Password;

namespace aliment_backend.Interfaces
{
    /// <summary>
    /// Interface pour le service d'authentification.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Connecte un utilisateur avec les informations de connexion spécifiées.
        /// </summary>
        /// <param name="login">Les informations de connexion de l'utilisateur.</param>
        /// <returns>Un objet ResponseToken contenant le résultat de la connexion.</returns>
        Task<ResponseToken> LoginAsync(Login login);

        /// <summary>
        /// Enregistre un nouvel utilisateur avec les informations spécifiées et le mot de passe créé.
        /// </summary>
        /// <param name="info">Les informations personnelles de l'utilisateur à enregistrer.</param>
        /// <param name="password">Le mot de passe créé pour l'utilisateur.</param>
        /// <returns>Un objet ResponseToken contenant le résultat de l'enregistrement.</returns>
        Task<ResponseToken> RegisterAsync(Info info, CreatePassword password);       
    }
}
