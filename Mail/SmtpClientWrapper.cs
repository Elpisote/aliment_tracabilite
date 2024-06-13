using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace aliment_backend.Mail
{
    public class SmtpClientWrapper
    {
        private readonly ISmtpClient _client;

        public SmtpClientWrapper(ISmtpClient client)
        {
            _client = client;
        }

        public Task ConnectAsync(string server, int port, SecureSocketOptions options, CancellationToken cancellationToken)
        {
            return _client.ConnectAsync(server, port, options, cancellationToken);
        }

        public Task AuthenticateAsync(string username, string password, CancellationToken cancellationToken)
        {
            return _client.AuthenticateAsync(username, password, cancellationToken);
        }

        public Task SendAsync(MimeMessage message, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _client.SendAsync(message, cancellationToken);
        }

        public Task DisconnectAsync(bool quit, CancellationToken cancellationToken)
        {
            return _client.DisconnectAsync(quit, cancellationToken);
        }
    }
}
