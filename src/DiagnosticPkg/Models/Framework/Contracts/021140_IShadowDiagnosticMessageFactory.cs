using System;

namespace Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Contracts
{
    /// <summary>
    /// Provides a factory interface for creating diagnostic messages
    /// based on context such as sender, category, message content, level, and timestamp.
    /// </summary>
    public interface IShadowDiagnosticMessageFactory
    {
        /// <summary>
        /// Creates a new diagnostic message instance.
        /// </summary>
        /// <param name="senderMeta">The metadata of the sender generating the diagnostic message.</param>
        /// <param name="category">The category used to classify the diagnostic message.</param>
        /// <param name="message">The content of the diagnostic message.</param>
        /// <param name="level">The severity level of the diagnostic message.</param>
        /// <param name="timestamp">The timestamp when the diagnostic message was created.</param>
        /// <returns>A diagnostic message instance.</returns>
        IShadowDiagnosticMessage Create(
            IShadowDiagnosableMeta senderMeta,
            string category,
            string message,
            ShadowDiagnosticLevel level,
            DateTimeOffset timestamp
        );

        /// <summary>
        /// Creates a new diagnostic message instance.
        /// </summary>
        /// <param name="senderMeta">The metadata of the sender generating the diagnostic message.</param>
        /// <param name="category">The category used to classify the diagnostic message.</param>
        /// <param name="message">The content of the diagnostic message.</param>
        /// <param name="level">The severity level of the diagnostic message.</param>
        /// <returns>A diagnostic message instance.</returns>
        IShadowDiagnosticMessage Create(
            IShadowDiagnosableMeta senderMeta,
            string category,
            string message,
            ShadowDiagnosticLevel level
        );

        /// <summary>
        /// Creates a new diagnostic message instance.
        /// </summary>
        /// <param name="senderMeta">The metadata of the sender generating the diagnostic message.</param>
        /// <param name="category">The category used to classify the diagnostic message.</param>
        /// <param name="message">The content of the diagnostic message.</param>
        /// <returns>A diagnostic message instance.</returns>
        IShadowDiagnosticMessage Create(
            IShadowDiagnosableMeta senderMeta,
            string category,
            string message
        );
    }
}
