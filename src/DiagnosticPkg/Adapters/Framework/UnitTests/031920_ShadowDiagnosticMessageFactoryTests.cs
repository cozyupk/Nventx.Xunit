using System;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Adapters.Framework.Impl;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Contracts;
using FluentAssertions;
using Moq;
using Xunit;

namespace Cozyupk.HelloShadowDI.DiagnosticPkg.Adapters.Framework.UnitTests
{
    /// <summary>
    /// Contains unit tests for the ShadowDiagnosticMessageFactory class, verifying correct message creation and validation logic.
    /// </summary>
    public class ShadowDiagnosticMessageFactoryTests
    {
#pragma warning disable IDE0079 // Remove unnecessary suppressions
#pragma warning disable CA1859 // Use concrete types where possible to improve performance
        private IShadowDiagnosticMessageFactory Factory { get; } = new ShadowDiagnosticMessageFactory();
#pragma warning restore CA1859 // Use concrete types where possible to improve performance
#pragma warning restore IDE0079 // Remove unnecessary suppressions

        /// <summary>
        /// Provides a reusable sender meta instance for use in test cases.
        /// </summary>
        private IShadowDiagnosableMeta SenderMeta { get; } = new Mock<IShadowDiagnosableMeta>().Object;

        /// <summary>
        /// Verifies that Create returns a message with all provided parameters correctly set.
        /// </summary>
        [Fact]
        public void Create_WithAllParameters_ShouldReturnCorrectMessage()
        {
            // Arrange
            var category = "TestCategory";
            var message = "This is a test message";
            var level = ShadowDiagnosticLevel.Warning;
            var timestamp = new DateTimeOffset(2024, 1, 1, 12, 34, 56, TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow));

            // Act
            var result = Factory.Create(SenderMeta, category, message, level, timestamp);

            // Assert
            result.SenderMeta.Should().Be(SenderMeta);
            result.Category.Should().Be(category);
            result.Message.Should().Be(message);
            result.Level.Should().Be(level);
            result.Timestamp.Should().Be(timestamp);
        }

        /// <summary>
        /// Verifies that Create sets the timestamp to the current time when not provided.
        /// </summary>
        [Fact]
        public void Create_WithoutTimestamp_ShouldSetTimestampToNow()
        {
            // Arrange
            var category = "TimeCategory";
            var message = "Timestamp should be now-ish";
            var level = ShadowDiagnosticLevel.Info;

            // Act
            var before = DateTimeOffset.Now;
            var result = Factory.Create(SenderMeta, category, message, level);
            var after = DateTimeOffset.Now;

            // Assert
            result.Timestamp.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
            result.Level.Should().Be(level);
        }

        /// <summary>
        /// Verifies that Create defaults the level to Info and timestamp to now when not provided.
        /// </summary>
        [Fact]
        public void Create_WithoutLevelAndTimestamp_ShouldDefaultToInfo_And_Now()
        {
            // Arrange
            var category = "DefaultCategory";
            var message = "Default behavior test";

            // Act
            var before = DateTimeOffset.Now;
            var result = Factory.Create(SenderMeta, category, message);
            var after = DateTimeOffset.Now;

            // Assert
            result.Level.Should().Be(ShadowDiagnosticLevel.Info);
            result.Timestamp.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
        }

        /// <summary>
        /// Verifies that Create throws an ArgumentNullException when the senderMeta parameter is null.
        /// </summary>
        [Fact]
        public void Create_ShouldThrowArgumentNullException_WhenSenderMetaIsNull()
        {
            // Arrange
            IShadowDiagnosableMeta? senderMeta = null;

            // Act
            Action act = () => Factory.Create(senderMeta!, "Cat", "msg");

            // Assert
            act.Should().Throw<ArgumentNullException>()
               .WithParameterName("senderMeta");
        }

        /// <summary>
        /// Verifies that Create throws an exception when the category parameter is null or empty.
        /// </summary>
        /// <param name="category">The category value to test.</param>
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        public void Create_WithInvalidCategory_ShouldThrow(string? category, Type expectedException)
        {
            // Act
            Action act = () => Factory.Create(SenderMeta, category!, "msg");

            // Assert
            act.Should().Throw<Exception>().Where(e => e.GetType() == expectedException);
        }

        /// <summary>
        /// Verifies that Create throws an exception when the message parameter is null.
        /// </summary>
        [Fact]
        public void Create_WithNullMessage_ShouldThrow()
        {
            // Act
            Action act = () => Factory.Create(SenderMeta, "cat", null!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .Where(e => e is ArgumentNullException || e is ArgumentException);
        }
    }
}