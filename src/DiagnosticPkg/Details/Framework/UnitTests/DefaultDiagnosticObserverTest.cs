using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Adapters.Framework.Impl;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Details.Framework.Impl;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Contracts;
using Xunit;

namespace Cozyupk.HelloShadowDI.DiagnosticPkg.Details.Framework.UnitTests
{
    /// <summary>
    /// Provides unit tests for the DefaultShadowDiagnosticObserver class.
    /// Verifies correct handling of diagnostic messages with various sender types and null values.
    /// </summary>
    public class DefaultDiagnosticObserverTests
    {
        /// <summary>
        /// Factory for creating diagnostic messages used in tests.
        /// </summary>
#pragma warning disable IDE0079
#pragma warning disable CA1859
        private IShadowDiagnosticMessageFactory Factory { get; } = new ShadowDiagnosticMessageFactory();
#pragma warning restore CA1859
#pragma warning restore IDE0079

        /// <summary>
        /// Captures the output written to Trace during the execution of the provided action.
        /// </summary>
        /// <param name="action">The action whose Trace output should be captured.</param>
        /// <returns>The captured Trace output as a string.</returns>
        private static string CaptureTraceOutput(Action action)
        {
            var sb = new StringBuilder();
            var writer = new StringWriter(sb);
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new TextWriterTraceListener(writer));
            action();
            Trace.Flush();
            return sb.ToString();
        }

        /// <summary>
        /// Simple sender class for testing sender type output.
        /// </summary>
        private class SimpleSender { }

        /// <summary>
        /// Sender class with custom ToString() for testing detailed sender output.
        /// </summary>
        private class DetailedSender
        {
            /// <summary>
            /// Returns a custom string for the sender.
            /// </summary>
            public override string ToString() => "Custom Details";
        }

        /// <summary>
        /// Verifies that the sender type name is included in the output when ToString() is not overridden.
        /// </summary>
        [Fact]
        public void OnDiagnostic_Includes_SenderType_WhenToStringIsNull()
        {
            var sender = new SimpleSender();
            var message = Factory.Create(sender, "Category", "Test message", ShadowDiagnosticLevel.Info);
            var observer = new DefaultShadowDiagnosticObserver();
            var output = CaptureTraceOutput(() => observer.OnDiagnostic(message));

#if DEBUG
            Assert.Contains("SimpleSender", output);
            Assert.DoesNotContain("Custom Details", output);
#else
            Assert.True(string.IsNullOrWhiteSpace(output));
#endif
        }

        /// <summary>
        /// Verifies that the sender's ToString() value is used in the output if it differs from the type name.
        /// </summary>
        [Fact]
        public void OnDiagnostic_Uses_ToString_IfDifferentFromTypeName()
        {
            var sender = new DetailedSender();
            var message = Factory.Create(sender, "Category", "Test message", ShadowDiagnosticLevel.Info);
            var observer = new DefaultShadowDiagnosticObserver();
            var output = CaptureTraceOutput(() => observer.OnDiagnostic(message));

#if DEBUG
            Assert.Contains("DetailedSender (Custom Details)", output);
#else
            Assert.True(string.IsNullOrWhiteSpace(output));
#endif
        }

        /// <summary>
        /// Verifies that "(Unknown)" is used in the output if the sender is null.
        /// </summary>
        [Fact]
        public void OnDiagnostic_Uses_Unknown_IfSenderIsNull()
        {
            var message = Factory.Create(null, "Category", "Test message", ShadowDiagnosticLevel.Info);
            var observer = new DefaultShadowDiagnosticObserver();
            var output = CaptureTraceOutput(() => observer.OnDiagnostic(message));

#if DEBUG
            Assert.Contains("(Unknown)", output);
#else
            Assert.True(string.IsNullOrWhiteSpace(output));
#endif
        }
    }
}