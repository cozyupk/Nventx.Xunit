using System;
using System.Diagnostics;
using System.IO;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Details.Framework.Impl;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Contracts;
using Moq;
using Xunit;

namespace Cozyupk.HelloShadowDI.DiagnosticPkg.Details.Framework.UnitTests
{
    /// <summary>
    /// Unit tests for DefaultShadowDiagnosticObserver.
    /// </summary>
    public class DefaultShadowDiagnosticObserverTests
    {
        /// <summary>
        /// Verifies that the constructor throws ArgumentNullException if the formatter is null.
        /// </summary>
        [Fact]
        public void Constructor_ThrowsIfFormatterIsNull()
        {
            // Arrange
            IShadowDiagnosticFormatter<string> formatter = null!;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new DefaultShadowDiagnosticObserver(formatter));
        }

        /// <summary>
        /// Verifies that OnDiagnostic throws ArgumentNullException if the message is null.
        /// </summary>
        [Fact]
        public void OnDiagnostic_ThrowsIfMessageIsNull()
        {
            // Arrange
            var mockFormatter = new Mock<IShadowDiagnosticFormatter<string>>();
            var observer = new DefaultShadowDiagnosticObserver(mockFormatter.Object);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                observer.OnDiagnostic(null!));
        }

        /// <summary>
        /// Verifies that OnDiagnostic writes the formatted message to Trace in DEBUG mode,
        /// and does not write in Release mode.
        /// </summary>
        [Fact]
        public void OnDiagnostic_WritesFormattedMessageToTrace()
        {
            // Arrange
            var mockFormatter = new Mock<IShadowDiagnosticFormatter<string>>();
            var mockMessage = new Mock<IShadowDiagnosticMessage>();
            var formatted = "[DEBUG] [2025-05-15] Test message";

            // Setup the formatter to return a specific formatted string
            mockFormatter.Setup(f => f.Format(It.IsAny<IShadowDiagnosticMessage>()))
                         .Returns(formatted);

            var observer = new DefaultShadowDiagnosticObserver(mockFormatter.Object);

            // Redirect Trace output to a StringWriter for assertion
            using var sw = new StringWriter();
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new TextWriterTraceListener(sw));

            // Act
            observer.OnDiagnostic(mockMessage.Object);
            Trace.Flush();

#if DEBUG
            // Assert: The formatted message should be present in the Trace output
            var output = sw.ToString();
            Assert.Contains(formatted, output);
#else
            // Assert: In Release mode, Trace output should be empty
            Assert.Empty(sw.ToString());        
#endif
        }
    }
}
