using Cozyupk.HelloShadowDI.Models.Contracts;
using Cozyupk.HelloShadowDI.Models.Impl;
using Xunit;

namespace UnitTests
{
    public class MessageModelTests
    {
        [Fact]
        public void Model_ShouldImplementExpectedInterfaces()
        {
            // Arrange
            MessageModel model = new();

            // Assert
            Assert.True(model is IMessageModel);
        }

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