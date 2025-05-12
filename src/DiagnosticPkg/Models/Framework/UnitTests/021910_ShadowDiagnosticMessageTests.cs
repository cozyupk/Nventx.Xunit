using System;
using Xunit;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Impl;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Contracts;

namespace Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="ShadowDiagnosticMessage"/> class.
    /// </summary>
    public class ShadowDiagnosticMessageTests
    {
        /// <summary>
        /// Verifies that the constructor correctly assigns all properties when valid arguments are provided.
        /// </summary>
        [Fact]
        public void Constructor_AssignsAllPropertiesCorrectly()
        {
            // Arrange
            var sender = new object();
            var category = "System";
            var message = "Something happened";
            var level = ShadowDiagnosticLevel.Warning;
            var timestamp = new DateTime(2023, 1, 1, 12, 0, 0);

            // Act
            var diag = new ShadowDiagnosticMessage(sender, category, message, level, timestamp);

            // Assert
            Assert.Equal(sender, diag.Sender);
            Assert.Equal(category, diag.Category);
            Assert.Equal(message, diag.Message);
            Assert.Equal(level, diag.Level);
            Assert.Equal(timestamp, diag.Timestamp);
        }

        /// <summary>
        /// Verifies that the constructor uses the current time when the timestamp is not provided.
        /// </summary>
        [Fact]
        public void Constructor_UsesCurrentTime_WhenTimestampIsNull()
        {
            // Arrange
            var before = DateTime.Now;
            var diag = new ShadowDiagnosticMessage(null, "Test", "Message");
            var after = DateTime.Now;

            // Assert
            Assert.InRange(diag.Timestamp, before, after);
        }

        /// <summary>
        /// Verifies that the constructor throws an <see cref="ArgumentException"/> when the category is null or empty.
        /// </summary>
        /// <param name="invalidCategory">The invalid category value to test.</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsException_WhenCategoryIsInvalid(string? invalidCategory)
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                new ShadowDiagnosticMessage(null, invalidCategory!, "Message"));
            Assert.Equal("Category cannot be null or empty. (Parameter 'category')", ex.Message);
        }

        /// <summary>
        /// Verifies that the constructor throws an <see cref="ArgumentException"/> when the message is null or empty.
        /// </summary>
        /// <param name="invalidMessage">The invalid message value to test.</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsException_WhenMessageIsInvalid(string? invalidMessage)
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                new ShadowDiagnosticMessage(null, "Test", invalidMessage!));
            Assert.Equal("Message cannot be null or empty. (Parameter 'message')", ex.Message);
        }
    }
}