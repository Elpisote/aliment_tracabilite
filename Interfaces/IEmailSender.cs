using aliment_backend.Mail;

namespace aliment_backend.Interfaces
{
    /// <summary>
    /// Interface pour envoyer des e-mails 
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Envoie un e-mail en utilisant les informations spécifiées dans l'objet <paramref name="message"/>.
        /// </summary>
        /// <param name="message">L'objet <see cref="Message"/> contenant les détails de l'e-mail à envoyer.</param>
        public void SendEmailAsync(Message message);
    }
}
