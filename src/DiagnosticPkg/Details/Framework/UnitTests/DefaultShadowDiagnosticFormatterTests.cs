using System;
using System.Collections.Generic;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Details.Framework.Impl;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Contracts;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Cozyupk.HelloShadowDI.DiagnosticPkg.Details.Framework.UnitTests
{
    /// <summary>
    /// Unit tests for DefaultShadowDiagnosticFormatter.
    /// Verifies that diagnostic messages are formatted correctly for various scenarios.
    /// </summary>
    public class DefaultShadowDiagnosticFormatterTests
    {
        private readonly ITestOutputHelper _output;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultShadowDiagnosticFormatterTests"/> class.
        /// </summary>
        /// <param name="output">The test output helper for logging test output.</param>
        public DefaultShadowDiagnosticFormatterTests(ITestOutputHelper output)
        {
            _output = output;
        }

        /// <summary>
        /// Ensures that all fields are included in the formatted message.
        /// </summary>
        [Fact]
        public void Format_IncludesAllFieldsInFormattedMessage()
        {
            // Arrange: Prepare a mock diagnostic message with all fields set.
            var message = new Mock<IShadowDiagnosticMessage>();
            var sender = new object();
            var timestamp = new DateTimeOffset(2025, 5, 15, 9, 30, 0, TimeSpan.FromHours(9));

            message.Setup(m => m.Level).Returns(ShadowDiagnosticLevel.Info);
            message.Setup(m => m.Sender).Returns(sender);
            message.Setup(m => m.Timestamp).Returns(timestamp);
            message.Setup(m => m.Category).Returns("Test.Category");
            message.Setup(m => m.Message).Returns("Hello from Bob.");

            var formatter = new DefaultShadowDiagnosticFormatter();

            // Act: Format the message.
            var formatted = formatter.Format(message.Object);

            // Assert: Check that all expected fields are present in the output.
            Assert.StartsWith("[INFO]", formatted);
            Assert.Contains("Test.Category", formatted);
            Assert.Contains("Hello from Bob.", formatted);
            Assert.Contains($"{sender.GetType().Name}", formatted);
            Assert.Contains(sender.GetHashCode().ToString("x"), formatted); // hash should be in hex
            Assert.Contains(timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff"), formatted);
        }

        /// <summary>
        /// Ensures that the correct prefix is used for each diagnostic level.
        /// </summary>
        /// <param name="level">The diagnostic level to test.</param>
        /// <param name="expectedPrefix">The expected prefix string.</param>
        [Theory]
        [InlineData(ShadowDiagnosticLevel.Trace, "[TRACE]")]
        [InlineData(ShadowDiagnosticLevel.Debug, "[DEBUG]")]
        [InlineData(ShadowDiagnosticLevel.Info, "[INFO]")]
        [InlineData(ShadowDiagnosticLevel.Notice, "[NOTICE]")]
        [InlineData(ShadowDiagnosticLevel.Warning, "[WARN]")]
        [InlineData(ShadowDiagnosticLevel.Error, "[ERROR]")]
        [InlineData(ShadowDiagnosticLevel.Critical, "[FATAL]")]
        public void Format_UsesCorrectPrefixForLevel(ShadowDiagnosticLevel level, string expectedPrefix)
        {
            // Arrange: Prepare a mock diagnostic message with the specified level.
            var message = new Mock<IShadowDiagnosticMessage>();
            message.Setup(m => m.Level).Returns(level);
            message.Setup(m => m.Sender).Returns(new object());
            message.Setup(m => m.Timestamp).Returns(DateTimeOffset.Now);
            message.Setup(m => m.Category).Returns("LevelTest");
            message.Setup(m => m.Message).Returns("Testing prefix");

            var formatter = new DefaultShadowDiagnosticFormatter();

            // Act: Format the message.
            var formatted = formatter.Format(message.Object);

            // Assert: Output should start with the expected prefix.
            Assert.StartsWith(expectedPrefix, formatted);
        }

        /// <summary>
        /// Custom sender class with overridden ToString for testing.
        /// </summary>
        private class CustomSenderWithCustomToString
        {
            /// <summary>
            /// Returns a custom description for the sender.
            /// </summary>
            public override string ToString() => "Overridden Description";
        }

        /// <summary>
        /// Custom sender class that returns its type name as string.
        /// </summary>
        private class CustomSenderWithNameString
        {
            /// <summary>
            /// Returns a custom description for the sender. (GetType().Name)
            /// </summary>
            public override string ToString() => this.GetType().Name;
        }

        /// <summary>
        /// Custom sender class that returns its full type name as string.
        /// </summary>
        private class CustomSenderWithFullNameString
        {
            /// <summary>
            /// Returns a custom description for the sender. (GetType().FullName)
            /// </summary>
            public override string ToString() => this.GetType()?.FullName ?? "";
        }

        /// <summary>
        /// Provides test data for sender formatting scenarios using strongly-typed MemberData.
        /// </summary>
        public static TheoryData<string, string> SenderTestData =>
            new()
            {
                { "NULL", "(Unknown)" },
                { "CustomSenderWithCustomToString", "(CustomSenderWithCustomToString (Overridden Description)/" },
                { "CustomSenderWithNameString", "(CustomSenderWithNameString/" },
                { "CustomSenderWithFullNameString", "(CustomSenderWithFullNameString/" }
            };

        /// <summary>
        /// Ensures that the sender's ToString is included in the formatted message if it differs from the type name.
        /// </summary>
        /// <param name="senderTypeName">The sender type name to test.</param>
        /// <param name="expectedString">The expected string to be found in the formatted output.</param>
        [Theory]
        [MemberData(nameof(SenderTestData))]
        public void Format_IncludesSenderToStringIfDifferentFromTypeName(string senderTypeName, string expectedString)
        {
            // Arrange: Use a custom sender with an overridden ToString method.
            object? sender = senderTypeName switch
            {
                "NULL" => null,
                "CustomSenderWithCustomToString" => new CustomSenderWithCustomToString(),
                "CustomSenderWithNameString" => new CustomSenderWithNameString(),
                "CustomSenderWithFullNameString" => new CustomSenderWithFullNameString(),
                _ => throw new ArgumentException("Invalid sender type name", nameof(senderTypeName))
            };

            var message = new Mock<IShadowDiagnosticMessage>();
            message.Setup(m => m.Sender).Returns(sender);
            message.Setup(m => m.Level).Returns(ShadowDiagnosticLevel.Error);
            message.Setup(m => m.Timestamp).Returns(DateTimeOffset.Now);
            message.Setup(m => m.Category).Returns("Test");
            message.Setup(m => m.Message).Returns("ToString override test.");

            var formatter = new DefaultShadowDiagnosticFormatter();

            // Act: Format the message.
            var formatted = formatter.Format(message.Object);

            _output.WriteLine($"{(sender?.ToString() ?? "[null]")}: {formatted}"); // For debugging purposes

            // Assert: Output should include both the type name and the overridden description.
            Assert.Contains(expectedString, formatted);
        }

        /// <summary>
        /// Ensures that a fallback prefix is used when the diagnostic level is unknown.
        /// </summary>
        [Fact]
        public void Format_UsesFallbackPrefixForUnknownLevel()
        {
            // Arrange
            var message = new Mock<IShadowDiagnosticMessage>();
            message.Setup(m => m.Level).Returns((ShadowDiagnosticLevel)(-1)); // Invalid value!
            message.Setup(m => m.Sender).Returns(new object());
            message.Setup(m => m.Timestamp).Returns(DateTimeOffset.Now);
            message.Setup(m => m.Category).Returns("FallbackTest");
            message.Setup(m => m.Message).Returns("Testing default");

            var formatter = new DefaultShadowDiagnosticFormatter();

            // Act
            var formatted = formatter.Format(message.Object);

            // Assert
            Assert.StartsWith("[DIAG]", formatted); // Evidence that the default case was used
        }
    }
}
