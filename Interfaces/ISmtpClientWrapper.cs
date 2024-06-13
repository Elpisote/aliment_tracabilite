using MailKit.Security;
using MimeKit;

namespace aliment_backend.Interfaces
{
    public interface ISmtpClientWrapper
    {
        Task ConnectAsync(string server, int port, SecureSocketOptions options, CancellationToken cancellationToken);
        Task AuthenticateAsync(string username, string password, CancellationToken cancellationToken);
        Task SendAsync(MimeMessage message, CancellationToken cancellationToken = default(CancellationToken));
        Task DisconnectAsync(bool quit, CancellationToken cancellationToken);
    }
}
