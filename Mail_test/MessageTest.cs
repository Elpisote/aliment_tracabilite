using aliment_backend.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unit_test.Mail_test
{
    public class MessageTest
    {
        [Fact]
        public void Message_Constructor_NullTo_ThrowsException()
        {
            // Arrange & Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() =>
            {
                // Act: Attempt to create a Message with null 'to'
                new Message(null!, "Test Subject", "Test Content");
            });

            // Assert
            Assert.Equal("to", ex.ParamName);
        }
    }
}
