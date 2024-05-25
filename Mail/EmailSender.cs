﻿using aliment_backend.Interfaces;
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
        /// <returns>Une tâche représentant l'opération asynchrone d'envoi de l'e-mail.</returns>
        public async Task SendEmailAsync(Message message)
        {
            var mailMessage = CreateEmailMessage(message);

            await SendAsync(mailMessage);
        }

        /// <summary>
        /// Crée un objet <see cref="MimeMessage"/> à partir des détails du message spécifié.
        /// </summary>
        /// <param name="message">Les détails du message à inclure dans l'e-mail.</param>
        /// <returns>Un objet <see cref="MimeMessage"/> représentant le message e-mail créé.</returns>
        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage(); 
            emailMessage.From.Add(new MailboxAddress("Resto_Ratatouille",_emailConfig.From));
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
            return emailMessage;
        }

        /// <summary>
        /// Envoie de manière asynchrone un message e-mail à l'aide des paramètres de configuration SMTP spécifiés.
        /// </summary>
        /// <param name="mailMessage">L'objet <see cref="MimeMessage"/> représentant le message e-mail à envoyer.</param>
        /// <returns>Une tâche représentant l'opération asynchrone d'envoi de l'e-mail.</returns>
        private async Task SendAsync(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            client.ServerCertificateValidationCallback = (s, c, h, e) => true; // Ignorer la validation du certificat 
            try
            {
                await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailConfig.Username, _emailConfig.Password);
                await client.SendAsync(mailMessage);
            }
            catch
            {
                Console.WriteLine("Erreur lors de l'envoi de l'e-mail");
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }       
    }
}
