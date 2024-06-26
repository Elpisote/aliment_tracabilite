using aliment_backend.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace aliment_backend.Mail
{
    /// <summary>
    /// Implémente l'interface <see cref="IEmailSender"/> pour envoyer des e-mails en utilisant la configuration spécifiée.
    /// </summary>
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        public Func<SmtpClient> SmtpClientFactory { get; set; } = () => new SmtpClient();

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="EmailSender"/> avec la configuration d'e-mail spécifiée.
        /// </summary>
        /// <param name="emailConfig">La configuration d'e-mail à utiliser pour l'envoi des e-mails.</param>
        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        /// <summary>
        /// Envoie de manière asynchrone un e-mail avec les détails spécifiés.
        /// </summary>
        /// <param name="message">Les détails du message à envoyer.</param>
        public void SendEmailAsync(Message message)
        {
            MimeMessage emailMessage = new();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.FromName, _emailConfig.FromEmail));
            emailMessage.To.Add(message.To);
            emailMessage.Subject = message.Subject;

            // Vérifier si le contenu du message est null
            if (message.Content != null)
            {
                // Si le contenu du message n'est pas null, créer une partie de texte HTML avec le contenu du message
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = string.Format(message.Content)
                };
            }
            else
            {
                // Si le contenu du message est null, initialiser le corps du message avec une chaîne vide
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = string.Empty
                };
            }
            using var smtp = SmtpClientFactory();
            smtp.Connect(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls, default);
            smtp.Authenticate(_emailConfig.Username, _emailConfig.Password);
            smtp.Send(emailMessage);
            smtp.Disconnect(true);
        }         
    }
}