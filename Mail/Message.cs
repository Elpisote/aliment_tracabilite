using MimeKit;

namespace aliment_backend.Mail
{
    /// <summary>
    /// Représente un message e-mail avec les détails spécifiés tels que le destinataire, le sujet et le contenu.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Obtient ou définit l'adresse e-mail du destinataire du message.
        /// </summary>
        public MailboxAddress To { get; set; }

        /// <summary>
        /// Obtient ou définit le sujet du message e-mail.
        /// </summary>
        public string? Subject { get; set; }

        /// <summary>
        /// Obtient ou définit le contenu du message e-mail.
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="Message"/> avec l'adresse e-mail du destinataire, le sujet et le contenu spécifiés.
        /// </summary>
        /// <param name="to">L'adresse e-mail du destinataire.</param>
        /// <param name="subject">Le sujet du message e-mail.</param>
        /// <param name="content">Le contenu du message e-mail.</param>
        public Message(MailboxAddress to, string subject, string content)
        {
            To = to ?? throw new ArgumentNullException(nameof(to));
            Subject = subject;
            Content = content;
        }
    }
}
