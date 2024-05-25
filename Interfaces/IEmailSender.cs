using aliment_backend.Mail;

namespace aliment_backend.Interfaces
{
    /// <summary>
    /// Interface pour envoyer des e-mails de manière asynchrone.
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Envoie un e-mail de manière asynchrone en utilisant les informations spécifiées dans l'objet <paramref name="message"/>.
        /// </summary>
        /// <param name="message">L'objet Message contenant les détails de l'e-mail à envoyer.</param>
        /// <returns>Une tâche représentant l'opération asynchrone d'envoi d'e-mail.</returns>
        Task SendEmailAsync(Message message);
    }
}
