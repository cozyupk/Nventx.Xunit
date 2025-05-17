using System;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Contracts;

namespace Cozyupk.HelloShadowDI.DiagnosticPkg.Adapters.Framework.Impl
{
    /// <summary>
    /// Represents a diagnostic message containing information about an event, its severity, and timestamp.
    /// </summary>
    public class ShadowDiagnosticMessage : IShadowDiagnosticMessage
    {
        /// <summary>
        /// Gets the sender meta information of the diagnostic message.
        /// This can be any object that is the source of the message.
        /// </summary>
        public IShadowDiagnosableMeta SenderMeta { get; }

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
        public DateTimeOffset Timestamp { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShadowDiagnosticMessage"/> class.
        /// </summary>
        /// <param name="senderMeta">The sender meta information of the diagnostic message. Cannot be null.</param>
        /// <param name="category">The category of the diagnostic message. Cannot be null or empty.</param>
        /// <param name="message">The content of the diagnostic message. Cannot be null or empty.</param>
        /// <param name="level">The severity level of the diagnostic message. Must be a valid <see cref="ShadowDiagnosticLevel"/> value.</param>
        /// <param name="timestamp">The timestamp when the diagnostic message was created. Cannot be null or in the future.</param>
        protected internal ShadowDiagnosticMessage(
            IShadowDiagnosableMeta senderMeta,
            string category,
            string message,
            ShadowDiagnosticLevel level,
            DateTimeOffset timestamp)
        {
            // Validate the senderMeta to ensure it is not null.
            if (senderMeta is null)
                throw new ArgumentNullException(nameof(senderMeta), "Sender meta cannot be null.");

            // Validate the message to ensure it is not null or empty.
            if (category is null)
                throw new ArgumentNullException(nameof(category));
            if (category.Length == 0)
                throw new ArgumentException("Category cannot be empty.", nameof(category));

            // Validate the message to ensure it is not null or empty.
            if (message is null)
                throw new ArgumentNullException(nameof(message));
            if (message.Length == 0)
                throw new ArgumentException("Category cannot be empty.", nameof(message));

            // Validate the level to ensure it is a valid enumeration value.
            if (!Enum.IsDefined(typeof(ShadowDiagnosticLevel), level))
            {
                throw new ArgumentOutOfRangeException(nameof(level), "Invalid diagnostic level.");
            }

            // Validate the timestamp to ensure it is not null.
            if (timestamp == null)
            {
                throw new ArgumentNullException(nameof(timestamp), "Timestamp cannot be null.");
            }

            // Validate the timestamp to ensure it is not in the future.
            if (timestamp > DateTimeOffset.Now)
            {
                throw new ArgumentOutOfRangeException(nameof(timestamp), "Timestamp cannot be in the future.");
            }

            // Set properties with provided values or defaults.
            SenderMeta = senderMeta;
            Category = category;
            Level = level;
            Message = message;
            Timestamp = timestamp;
        }
    }
}
