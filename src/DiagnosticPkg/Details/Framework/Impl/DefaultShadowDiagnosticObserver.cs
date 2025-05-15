using System;
using System.Diagnostics;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Details.Framework.CompositionContracts;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Contracts;

namespace Cozyupk.HelloShadowDI.DiagnosticPkg.Details.Framework.Impl
{
    /// <summary>
    /// A default diagnostic observer that writes diagnostic messages to the debug output.
    /// This implementation formats messages with a timestamp and severity level prefix.
    /// It is only active in Debug builds; no output will be generated in Release builds.
    /// </summary>
    public class DefaultShadowDiagnosticObserver : IDefaultShadowDiagnosticObserver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultShadowDiagnosticObserver"/> class.
        /// Ensures that a non-null formatter is provided for formatting diagnostic messages.
        /// </summary>
        public DefaultShadowDiagnosticObserver(IShadowDiagnosticFormatter<string> formatter)
        {
            Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter), "Formatter cannot be null.");
        }

        /// <summary>
        /// Gets the formatter used to convert diagnostic messages into string output.
        /// </summary>
        IShadowDiagnosticFormatter<string> Formatter { get; }

        /// <summary>
        /// Handles a diagnostic message by formatting it with a timestamp and severity level,
        /// and outputs it to the debug output. This is only active in Debug builds.
        /// </summary>
        public void OnDiagnostic(IShadowDiagnosticMessage message)
        {
            // Check if the message is null and throw an exception if it is.
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null.");
            }

#if DEBUG
            // Outputs the diagnostic message to the debug output.
            // This is only active in Debug builds; no output will be generated in Release builds.
            var formatted = Formatter.Format(message);
            Trace.WriteLine(formatted);
#endif
        }
    }
}