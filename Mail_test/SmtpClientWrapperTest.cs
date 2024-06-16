using aliment_backend.Mail;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Moq;

namespace unit_test.Mail_test
{
    public class SmtpClientWrapperTest
    {
        private readonly Mock<ISmtpClient> _mockClient;
        private readonly SmtpClientWrapper _wrapper;

        public SmtpClientWrapperTest()
        {
            _mockClient = new Mock<ISmtpClient>();
            _wrapper = new SmtpClientWrapper(_mockClient.Object);
        }

        [Theory]
        [InlineData("localhost", 587, SecureSocketOptions.StartTls)]
        public async Task ConnectAsync_Success(string server, int port, SecureSocketOptions options)
        {
            // Act
            await _wrapper.ConnectAsync(server, port, options, CancellationToken.None);

            // Assert
            _mockClient.Verify(c => c.ConnectAsync(server, port, options, CancellationToken.None), Times.Once);
        }

        [Theory]
        [InlineData("test@example.com", "password")]
        public async Task AuthenticateAsync_Success(string username, string password)
        {
            // Act
            await _wrapper.AuthenticateAsync(username, password, CancellationToken.None);

            // Assert
            _mockClient.Verify(c => c.AuthenticateAsync(username, password, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task SendAsync_Success()
        {
            // Arrange
            var message = CreateSampleMessage();

            // Act
            await _wrapper.SendAsync(message, CancellationToken.None);

            // Assert
            _mockClient.Verify(c => c.SendAsync(message, CancellationToken.None, null!), Times.Once);
        }

        [Fact]
        public async Task DisconnectAsync_Success()
        {
            // Act
            await _wrapper.DisconnectAsync(true, CancellationToken.None);

            // Assert
            _mockClient.Verify(c => c.DisconnectAsync(true, CancellationToken.None), Times.Once);
        }

        private MimeMessage CreateSampleMessage()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("sender", "sender@example.com"));
            message.To.Add(new MailboxAddress("recipient", "recipient@example.com"));
            message.Subject = "Test Subject";
            message.Body = new TextPart("plain")
            {
                Text = "This is a test message."
            };
            return message;
        }
    }
}
