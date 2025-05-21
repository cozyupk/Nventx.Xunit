using System;
using FluentAssertions;
using Xunit;
using Cozyupk.Shadow.Flow.DiagnosticPkg.Adapters.Framework.Impl;
using Cozyupk.Shadow.Flow.DiagnosticPkg.Models.Framework.Contracts;
using Moq;

namespace Cozyupk.Shadow.Flow.DiagnosticPkg.Adapters.Framework.UnitTests
{

    /// <summary>
    /// Unit tests for the ShadowDiagnosticMessage class.
    /// </summary>
    public class ShadowDiagnosticMessageTests
    {
        /// <summary>
        /// Provides a reusable sender meta instance for use in test cases.
        /// </summary>
        private IShadowDiagnosableMeta SenderMeta { get; } = new Mock<IShadowDiagnosableMeta>().Object;

        /// <summary>
        /// Verifies that the constructor sets all properties correctly when valid arguments are provided.
        /// </summary>
        [Fact]
        public void Constructor_SetsPropertiesCorrectly()
        {
            var category = "TestCategory";
            var message = "This is a test message.";
            var level = ShadowDiagnosticLevel.Warning;
            var timestamp = DateTimeOffset.Now;

            // Act
            var diagMessage = new ShadowDiagnosticMessage(SenderMeta, category, message, level, timestamp);

            // Assert
            diagMessage.SenderMeta.Should().Be(SenderMeta);
            diagMessage.Category.Should().Be(category);
            diagMessage.Message.Should().Be(message);
            diagMessage.Level.Should().Be(level);
            diagMessage.Timestamp.Should().Be(timestamp);
        }

        /// <summary>
        /// Verifies that the constructor throws ArgumentException when the category is null or empty.
        /// </summary>
        /// <param name="invalidCategory">Invalid category value (null or empty string).</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsArgumentException_WhenCategoryIsInvalid(string? invalidCategory)
        {
            // Act
            var act = () => new ShadowDiagnosticMessage(
                senderMeta: SenderMeta,
                category: invalidCategory!,
                message: "msg",
                level: ShadowDiagnosticLevel.Info,
                timestamp: DateTimeOffset.Now
            );

            // Assert
            act.Should().Throw<ArgumentException>().WithParameterName("category");
        }

        /// <summary>
        /// Verifies that the constructor throws ArgumentException when the message is null or empty.
        /// </summary>
        /// <param name="invalidMessage">Invalid message value (null or empty string).</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsArgumentException_WhenMessageIsInvalid(string? invalidMessage)
        {
            // Act
            var act = () => new ShadowDiagnosticMessage(
                senderMeta: SenderMeta,
                category: "cat",
                message: invalidMessage!,
                level: ShadowDiagnosticLevel.Info,
                timestamp: DateTimeOffset.Now
            );

            // Assert
            act.Should().Throw<ArgumentException>().WithParameterName("message");
        }

        /// <summary>
        /// Verifies that the constructor throws ArgumentOutOfRangeException when the level is invalid.
        /// </summary>
        [Fact]
        public void Constructor_ThrowsArgumentOutOfRangeException_WhenLevelIsInvalid()
        {
            var invalidLevel = (ShadowDiagnosticLevel)(-1);

            // Act
            var act = () => new ShadowDiagnosticMessage(
                senderMeta: SenderMeta,
                category: "cat",
                message: "msg",
                level: invalidLevel,
                timestamp: DateTimeOffset.Now
            );

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("level");
        }

        /// <summary>
        /// Verifies that the constructor throws ArgumentOutOfRangeException when the timestamp is in the future.
        /// </summary>
        [Fact]
        public void Constructor_ThrowsArgumentOutOfRangeException_WhenTimestampIsInFuture()
        {
            var futureTime = DateTimeOffset.Now.AddMinutes(1);

            // Act
            var act = () => new ShadowDiagnosticMessage(
                senderMeta: SenderMeta,
                category: "cat",
                message: "msg",
                level: ShadowDiagnosticLevel.Info,
                timestamp: futureTime
            );

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("timestamp");
        }
    }
}