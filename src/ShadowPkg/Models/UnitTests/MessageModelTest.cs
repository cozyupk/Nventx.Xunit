using Cozyupk.HelloShadowDI.Models.Contracts;
using Cozyupk.HelloShadowDI.Models.Impl;
using Xunit;

namespace UnitTests
{
    public class MessageModelTests
    {
        /// <summary>
        /// Verifies that the MessageModel class implements the expected interface IMessageModel.
        /// </summary>
        [Fact]
        public void Model_ShouldImplementExpectedInterfaces()
        {
            // Arrange
            MessageModel model = new();

            // Assert
            Assert.True(model is IMessageModel);
        }

        /// <summary>
        /// Ensures that the Message property of the MessageModel class returns the expected text "Hello world.".
        /// </summary>
        [Fact]
        public void Message_ShouldReturnExpectedText()
        {
            // Arrange
            MessageModel model = new();

            // Act
            var result = model.Message;

            // Assert
            Assert.Equal("Hello world.", result);
        }
    }
}