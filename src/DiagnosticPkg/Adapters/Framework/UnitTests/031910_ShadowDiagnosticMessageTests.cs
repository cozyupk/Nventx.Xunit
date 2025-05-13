using System;
using FluentAssertions;
using Xunit;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Adapters.Framework.Impl;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Contracts;

namespace Cozyupk.HelloShadowDI.DiagnosticPkg.Adapters.Framework.UnitTests
{
    public class ShadowDiagnosticMessageTests
    {
        [Fact]
        public void Constructor_SetsPropertiesCorrectly()
        {
            var sender = new object();
            var category = "TestCategory";
            var message = "This is a test message.";
            var level = ShadowDiagnosticLevel.Warning;
            var timestamp = DateTimeOffset.Now;

            var diagMessage = new ShadowDiagnosticMessage(sender, category, message, level, timestamp);

            diagMessage.Sender.Should().Be(sender);
            diagMessage.Category.Should().Be(category);
            diagMessage.Message.Should().Be(message);
            diagMessage.Level.Should().Be(level);
            diagMessage.Timestamp.Should().Be(timestamp);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsArgumentException_WhenCategoryIsInvalid(string? invalidCategory)
        {
            var act = () => new ShadowDiagnosticMessage(
                sender: new object(),
                category: invalidCategory!,
                message: "msg",
                level: ShadowDiagnosticLevel.Info,
                timestamp: DateTimeOffset.Now
            );

            act.Should().Throw<ArgumentException>().WithParameterName("category");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_ThrowsArgumentException_WhenMessageIsInvalid(string? invalidMessage)
        {
            var act = () => new ShadowDiagnosticMessage(
                sender: new object(),
                category: "cat",
                message: invalidMessage!,
                level: ShadowDiagnosticLevel.Info,
                timestamp: DateTimeOffset.Now
            );

            act.Should().Throw<ArgumentException>().WithParameterName("message");
        }

        [Fact]
        public void Constructor_ThrowsArgumentOutOfRangeException_WhenLevelIsInvalid()
        {
            var invalidLevel = (ShadowDiagnosticLevel)(-1);

            var act = () => new ShadowDiagnosticMessage(
                sender: new object(),
                category: "cat",
                message: "msg",
                level: invalidLevel,
                timestamp: DateTimeOffset.Now
            );

            act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("level");
        }

        [Fact]
        public void Constructor_ThrowsArgumentOutOfRangeException_WhenTimestampIsInFuture()
        {
            var futureTime = DateTimeOffset.Now.AddMinutes(1);

            var act = () => new ShadowDiagnosticMessage(
                sender: new object(),
                category: "cat",
                message: "msg",
                level: ShadowDiagnosticLevel.Info,
                timestamp: futureTime
            );

            act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("timestamp");
        }
    }
}