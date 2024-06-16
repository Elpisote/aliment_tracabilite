using aliment_backend.Interfaces;
using aliment_backend.Mail;
using MailKit.Security;
using MimeKit;
using Moq;
using System.Reflection;


namespace unit_test.Mail_test
{
    public class EmailSenderTest
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly Mock<ISmtpClientWrapper> _smtpClientMock;
        private readonly EmailSender _emailSender;

        public EmailSenderTest()
        {
            _emailConfig = new EmailConfiguration
            {
                SmtpServer = "localhost",
                Port = 587,
                Username = "test@example.com",
                Password = "password",
                FromEmail = "from@example.com",
                FromName = "Example"
            };
            _smtpClientMock = new Mock<ISmtpClientWrapper>();
            _emailSender = new EmailSender(_emailConfig, _smtpClientMock.Object);
        }

        [Fact]
        public async Task SendEmailAsync_SendEmail()
        {
            // Arrange
            Message? message = new(new MailboxAddress(_emailConfig.FromName, _emailConfig.FromEmail), "Test Subject", "Test Content");

            MethodInfo? createEmailMessageMethod = typeof(EmailSender).GetMethod("CreateEmailMessage", BindingFlags.NonPublic | BindingFlags.Instance);
            MimeMessage? emailMessage = createEmailMessageMethod?.Invoke(_emailSender, new object[] { message }) as MimeMessage;

            if(_emailConfig.SmtpServer != null && _emailConfig.Username != null 
                && _emailConfig.Password != null && _emailConfig.FromEmail != null) 
            {
                _smtpClientMock.Setup(c => c.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls, CancellationToken.None))
                  .Returns(Task.CompletedTask);
                _smtpClientMock.Setup(c => c.AuthenticateAsync(_emailConfig.Username, _emailConfig.Password, CancellationToken.None))
                               .Returns(Task.CompletedTask);
                _smtpClientMock.Setup(c => c.SendAsync(It.IsAny<MimeMessage>(), CancellationToken.None))
                               .Returns(Task.CompletedTask);
                _smtpClientMock.Setup(c => c.DisconnectAsync(true, CancellationToken.None))
                               .Returns(Task.CompletedTask);

                // Act
                await _emailSender.SendEmailAsync(message);

                // Assert
                Assert.NotNull(emailMessage);
                Assert.Single(emailMessage.From);
                Assert.Equal(_emailConfig.FromName, emailMessage.From[0].Name);
                Assert.Contains(_emailConfig.FromEmail, emailMessage.From[0].ToString());                Assert.Equal(message.To, emailMessage.To[0]); // Check recipient
                Assert.Equal(message.Subject, emailMessage.Subject); 
                Assert.Equal(message.Content ?? string.Empty, emailMessage.HtmlBody);

                _smtpClientMock.Verify(c => c.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls, CancellationToken.None), Times.Once);
                _smtpClientMock.Verify(c => c.AuthenticateAsync(_emailConfig.Username, _emailConfig.Password, CancellationToken.None), Times.Once);
                _smtpClientMock.Verify(c => c.SendAsync(It.IsAny<MimeMessage>(), CancellationToken.None), Times.Once);
                _smtpClientMock.Verify(c => c.DisconnectAsync(true, CancellationToken.None), Times.Once);
            }  
        }

        [Fact]
        public void CreateEmailMessage_NullContent()
        {
            // Arrange
            Message? message = new(new MailboxAddress("Recipient", "recipient@example.com"), "Test Subject", null!);
            MethodInfo? createEmailMessageMethod = typeof(EmailSender).GetMethod("CreateEmailMessage", BindingFlags.NonPublic | BindingFlags.Instance);
            
            // Vérifiez si la méthode a été trouvée
            Assert.NotNull(createEmailMessageMethod);

            // Act 
            MimeMessage? emailMessage = createEmailMessageMethod.Invoke(_emailSender, new object[] { message }) as MimeMessage;

            // Assert
            Assert.NotNull(emailMessage);
            Assert.Single(emailMessage.From); 
            Assert.Single(emailMessage.To); 
            Assert.Equal(string.Empty, emailMessage.HtmlBody); 
        }       

        [Fact]
        public async Task SendAsync_SmptServerNull_ThrowsException()
        {
            // Arrange
            EmailConfiguration invalidConfig = new() 
            {
                SmtpServer = null,
                Port = 587,
                Username = "test@example.com",
                Password = "password",
                FromEmail = "from@example.com",
                FromName = "Example"
            };
            EmailSender emailSender = new(invalidConfig, _smtpClientMock.Object);
            Message message = new(new MailboxAddress("Recipient", "recipient@example.com"), "Test Subject", "Test Content");

            // Act 
            InvalidOperationException ex = await Assert.ThrowsAsync<InvalidOperationException>(() => emailSender.SendEmailAsync(message));
            
            // Assert 
            Assert.Contains("Le serveur SMTP n'est pas configuré.", ex.Message);
        }

        [Fact]
        public async Task SendAsync_UsernameNull_ThrowsException()
        {
            // Arrange
            EmailConfiguration invalidConfig = new()
            {
                SmtpServer = "localhost",
                Port = 587,
                Username = null,
                Password = "password",
                FromEmail = "from@example.com",
                FromName = "Example"
            };
            EmailSender emailSender = new(invalidConfig, _smtpClientMock.Object);
            Message message = new(new MailboxAddress("Recipient", "recipient@example.com"), "Test Subject", "Test Content");

            // Act 
            InvalidOperationException ex = await Assert.ThrowsAsync<InvalidOperationException>(() => emailSender.SendEmailAsync(message));

            // Assert           
            Assert.Contains("Le nom d'utilisateur SMTP n'est pas configuré.", ex.Message);
        }

        [Fact]
        public async Task SendAsync_PasswordNull_ThrowsException()
        {
            // Arrange
            EmailConfiguration invalidConfig = new()
            {
                SmtpServer = "localhost",
                Port = 587,
                Username = "test@example.com",
                Password = null,
                FromEmail = "from@example.com",
                FromName = "Example"
            };
            EmailSender emailSender = new(invalidConfig, _smtpClientMock.Object);
            Message message = new(new MailboxAddress("Recipient", "recipient@example.com"), "Test Subject", "Test Content");

            // Act 
            InvalidOperationException ex = await Assert.ThrowsAsync<InvalidOperationException>(() => emailSender.SendEmailAsync(message));

            // Assert           
            Assert.Contains("Le mot de passe SMTP n'est pas configuré.", ex.Message);

            _smtpClientMock.Verify(c => c.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<SecureSocketOptions>(), It.IsAny<CancellationToken>()), Times.Never);
            _smtpClientMock.Verify(c => c.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _smtpClientMock.Verify(c => c.SendAsync(It.IsAny<MimeMessage>(), It.IsAny<CancellationToken>()), Times.Never);
            _smtpClientMock.Verify(c => c.DisconnectAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}


