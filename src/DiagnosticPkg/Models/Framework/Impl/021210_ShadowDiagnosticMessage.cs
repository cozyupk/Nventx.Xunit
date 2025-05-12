using System;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Contracts;

namespace Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Impl
{
    /// <summary>
    /// Represents a diagnostic message containing information about an event, its severity, and timestamp.
    /// </summary>
    public class ShadowDiagnosticMessage : IShadowDiagnosticMessage
    {
        /// <summary>
        /// Gets the sender of the diagnostic message.
        /// This can be any object that is the source of the message.
        /// </summary>
        public object? Sender { get; }

        /// <summary>
        /// Gets the category of the diagnostic message.
        /// Categories are used to group or classify messages for easier filtering or analysis.
        /// </summary>
        public string Category { get; }

        /// <summary>
        /// Gets the severity level of the diagnostic message.
        /// </summary>
        public ShadowDiagnosticLevel Level { get; }

        /// <summary>
        /// Gets the content of the diagnostic message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the timestamp when the diagnostic message was created.
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShadowDiagnosticMessage"/> class.
        /// </summary>
        /// <param name="sender">The source of the diagnostic message. Can be null if the sender is not specified.</param>
        /// <param name="category">The category of the diagnostic message. Cannot be null or empty.</param>
        /// <param name="level">The severity level of the diagnostic message. Defaults to <see cref="ShadowDiagnosticLevel.Info"/> if not specified.</param>
        /// <param name="message">The content of the diagnostic message. Cannot be null or empty.</param>
        /// <param name="timestamp">The timestamp of the diagnostic message. If null, the current system time is used.</param>
        public ShadowDiagnosticMessage(
            object? sender,
            string category,
            string message,
            ShadowDiagnosticLevel level = ShadowDiagnosticLevel.Info,
            DateTime? timestamp = null)
        {
            // Validate the message to ensure it is not null or empty.
            if (string.IsNullOrEmpty(category))
            {
                throw new ArgumentException("Category cannot be null or empty.", nameof(category));
            }

            // Validate the message to ensure it is not null or empty.
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("Message cannot be null or empty.", nameof(message));
            }

            // Set properties with provided values or defaults.
            Sender = sender;
            Category = category;
            Level = level;
            Message = message;
            Timestamp = timestamp ?? DateTime.Now;
        }
    }
}
