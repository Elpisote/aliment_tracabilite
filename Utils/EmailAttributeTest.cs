using aliment_backend.Utils;

namespace unit_test.Utils
{
    public class EmailAttributeTest
    {
        private readonly EmailAttribute _emailAttribute = new();

        [Theory]
        [InlineData("test@example.com")]
        [InlineData("user123@gmail.com")]
        [InlineData("first.last@company.co.uk")]
        public void IsValid_ValidEmail_ReturnsTrue(string email)
        {
            // Act
            bool isValid = _emailAttribute.IsValid(email);

            // Assert
            Assert.True(isValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("invalidemail")]
        [InlineData("user@invalid")]
        [InlineData("user@domain")]
        [InlineData("user@domain.")]
        public void IsValid_InvalidEmail_ReturnsFalse(string email)
        {
            // Act
            bool isValid = _emailAttribute.IsValid(email);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void IsValid_NullEmail_ReturnsFalse()
        {
            // Act
            bool isValid = _emailAttribute.IsValid(null);

            // Assert
            Assert.False(isValid);
        }
    }
}
