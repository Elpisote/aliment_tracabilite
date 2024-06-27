using aliment_backend.Mail;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Moq;
using Moq.Protected;
using System.Reflection;


namespace unit_test.Mail_test
{
    public class EmailSenderTest
    {
        private readonly EmailConfiguration emailConfig;
        private readonly Mock<EmailSender> mockEmailSender;
        private readonly EmailSender emailSender;

        public EmailSenderTest()
        {
            emailConfig = new()
            {
                FromName = "Test Sender",
                FromEmail = "sender@example.com",
                SmtpServer = "smtp.example.com",
                Port = 587,
                Username = "username",
                Password = "password"
            };
            mockEmailSender = new(emailConfig) { CallBase = true };
            emailSender = mockEmailSender.Object;
        }

        [Fact]
        public async Task SendEmailAsync_ShouldSendEmailAsync()
        {
            // Arrange   
            Message message = new(new MailboxAddress("Recipient", "recipient@example.com"), "Test Subject", "This is a test email.");

            MimeMessage capturedMessage = null!;
            mockEmailSender.Protected()
                .Setup<Task>("SendEmailInternalAsync", ItExpr.IsAny<MimeMessage>())
                .Callback<MimeMessage>(msg => capturedMessage = msg)
                .Returns(Task.CompletedTask);          

            // Act
            await emailSender.SendEmailAsync(message);

            // Assert
            mockEmailSender.Protected().Verify<Task>("SendEmailInternalAsync", Times.Once(), ItExpr.IsAny<MimeMessage>());

            Assert.NotNull(capturedMessage);
            Assert.Equal(emailConfig.FromName, capturedMessage.From[0].Name);
            if(emailConfig.FromEmail != null)
                Assert.Contains(emailConfig.FromEmail, capturedMessage.From.ToString());

            Assert.Equal(message.To.Name, capturedMessage.To[0].Name);
            Assert.Equal(message.Subject, capturedMessage.Subject);
            if (message.Content != null)
            {
                Assert.Contains(message.Content, capturedMessage.HtmlBody);
            }
        }

        [Fact]
        public async Task SendEmailAsync_ShouldSendEmailWithoutContentAsync()
        {
            // Arrange     
            Message message = new(new MailboxAddress("Recipient", "recipient@example.com"), "Test Subject", null!);
            
            MimeMessage capturedMessage = null!;
            mockEmailSender.Protected()
                .Setup<Task>("SendEmailInternalAsync", ItExpr.IsAny<MimeMessage>())
                .Callback<MimeMessage>(msg => capturedMessage = msg)
                .Returns(Task.CompletedTask);

            // Act
            await emailSender.SendEmailAsync(message);

            // Assert
            mockEmailSender.Protected().Verify<Task>("SendEmailInternalAsync", Times.Once(), ItExpr.IsAny<MimeMessage>());
            Assert.NotNull(capturedMessage);
            Assert.Equal(emailConfig.FromName, capturedMessage.From[0].Name);
            if (emailConfig.FromEmail != null)
                Assert.Contains(emailConfig.FromEmail, capturedMessage.From.ToString());
            Assert.Equal(message.To.Name, capturedMessage.To[0].Name);
            Assert.Equal(message.Subject, capturedMessage.Subject);
            Assert.Equal(string.Empty, capturedMessage.HtmlBody);
        }
    }
}


