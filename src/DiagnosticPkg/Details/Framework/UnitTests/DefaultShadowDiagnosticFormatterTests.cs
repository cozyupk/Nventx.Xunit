using System;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Details.Framework.Impl;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Contracts;
using Moq;
using Xunit;

namespace Cozyupk.HelloShadowDI.DiagnosticPkg.Details.Framework.UnitTests
{
    public class DefaultShadowDiagnosticFormatterTests
    {
        private readonly DefaultShadowDiagnosticFormatter _formatter = new();

        [Fact]
        public void Format_CreatesFormattedString_WithExpectedComponents()
        {
            // Arrange
            var mockMessage = new Mock<IShadowDiagnosticMessage>();
            mockMessage.Setup(m => m.Level).Returns(ShadowDiagnosticLevel.Info);
            mockMessage.Setup(m => m.Timestamp).Returns(new DateTimeOffset(2025, 5, 15, 10, 30, 0, TimeSpan.FromHours(9)));
            mockMessage.Setup(m => m.Category).Returns("System.Test");
            mockMessage.Setup(m => m.Message).Returns("Something happened.");

            var mockMeta = new Mock<IShadowDiagnosableMeta>();
            mockMeta.Setup(m => m.Label).Returns("MySender/abcd");

            mockMessage.Setup(m => m.SenderMeta).Returns(mockMeta.Object);

            // Act
            var result = _formatter.Format(mockMessage.Object);

            // Assert
            Assert.StartsWith("[INFO]    [2025-05-15 10:30:00.000 +09:00] [System.Test] (MySender/abcd) Something happened.", result);
        }

        [Theory]
        [InlineData(ShadowDiagnosticLevel.Trace, "[TRACE]")]
        [InlineData(ShadowDiagnosticLevel.Debug, "[DEBUG]")]
        [InlineData(ShadowDiagnosticLevel.Info, "[INFO]")]
        [InlineData(ShadowDiagnosticLevel.Notice, "[NOTICE]")]
        [InlineData(ShadowDiagnosticLevel.Warning, "[WARN]")]
        [InlineData(ShadowDiagnosticLevel.Error, "[ERROR]")]
        [InlineData(ShadowDiagnosticLevel.Critical, "[FATAL]")]
        public void Format_PrefixMatchesLogLevel(ShadowDiagnosticLevel level, string expectedPrefix)
        {
            // Arrange
            var mockMessage = new Mock<IShadowDiagnosticMessage>();
            mockMessage.Setup(m => m.Level).Returns(level);
            mockMessage.Setup(m => m.Timestamp).Returns(DateTimeOffset.Now);
            mockMessage.Setup(m => m.Category).Returns("X");
            mockMessage.Setup(m => m.Message).Returns("msg");
            mockMessage.Setup(m => m.SenderMeta.Label).Returns("Sender");

            // Act
            var result = _formatter.Format(mockMessage.Object);

            // Assert
            Assert.StartsWith(expectedPrefix, result);
        }

        [Fact]
        public void Format_UsesFallbackPrefix_ForUnknownLevel()
        {
            // Arrange
            var mockMessage = new Mock<IShadowDiagnosticMessage>();
            mockMessage.Setup(m => m.Level).Returns((ShadowDiagnosticLevel)999);
            mockMessage.Setup(m => m.Timestamp).Returns(DateTimeOffset.UtcNow);
            mockMessage.Setup(m => m.Category).Returns("X");
            mockMessage.Setup(m => m.Message).Returns("unknown");
            mockMessage.Setup(m => m.SenderMeta.Label).Returns("Sender");

            // Act
            var result = _formatter.Format(mockMessage.Object);

            // Assert
            Assert.StartsWith("[DIAG]", result);
        }
    }
}
