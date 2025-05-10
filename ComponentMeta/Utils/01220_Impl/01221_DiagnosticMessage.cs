using System;
using Cozyupk.HelloShadowDI.ComponentMeta.Utils.Contracts;

namespace Cozyupk.HelloShadowDI.ComponentMeta.Utils.Impl
{
    /// <summary>
    /// Represents a diagnostic message containing information about an event, its severity, and timestamp.
    /// </summary>
    public class DiagnosticMessage : IShadowDiagnosticMessage
    {
        /// <summary>
        /// Gets the content of the diagnostic message.
        /// </summary>
        public string Message { get; } = "";

        /// <summary>
        /// Gets the severity level of the diagnostic message.
        /// </summary>
        public ShadowDiagnosticLevel Level { get; }

        /// <summary>
        /// Gets the timestamp when the diagnostic message was created.
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticMessage"/> class.
        /// </summary>
        /// <param name="message">The content of the diagnostic message.</param>
        /// <param name="level">The severity level of the diagnostic message. Default is <see cref="ShadowDiagnosticLevel.Info"/>.</param>
        /// <param name="timestamp">The timestamp of the diagnostic message. If null, the current time is used.</param>
        public DiagnosticMessage(string message, ShadowDiagnosticLevel level = ShadowDiagnosticLevel.Info, DateTime? timestamp = null)
        {
            Message = message;
            Level = level;
            Timestamp = timestamp ?? DateTime.Now;
        }
    }
}
