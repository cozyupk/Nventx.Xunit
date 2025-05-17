using System;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Contracts;

namespace Cozyupk.HelloShadowDI.DiagnosticPkg.Adapters.Framework.Impl
{
    /// <summary>
    /// Factory class for creating instances of <see cref="IShadowDiagnosticMessage"/>.
    /// </summary>
    public class ShadowDiagnosticMessageFactory : IShadowDiagnosticMessageFactory
    {
        /// <summary>
        /// Creates a new <see cref="IShadowDiagnosticMessage"/> instance with the specified parameters.
        /// </summary>
        /// <param name="senderMeta">The metadata of the sender generating the diagnostic message.</param>
        /// <param name="category">The category of the diagnostic message.</param>
        /// <param name="message">The content of the diagnostic message.</param>
        /// <param name="level">The severity level of the diagnostic message. Defaults to Info.</param>
        /// <param name="timestamp">The timestamp of the diagnostic message. If null, the current time is used.</param>
        /// <returns>A new instance of <see cref="IShadowDiagnosticMessage"/>.</returns>
        public IShadowDiagnosticMessage Create(
            IShadowDiagnosableMeta senderMeta,
            string category,
            string message,
            ShadowDiagnosticLevel level,
            DateTimeOffset timestamp)
        {
            // Create and return a new diagnostic message with the provided parameters.
            return new ShadowDiagnosticMessage(senderMeta, category, message, level, timestamp);
        }

        /// <summary>
        /// Creates a new <see cref="IShadowDiagnosticMessage"/> instance with the specified parameters.
        /// </summary>
        /// <param name="senderMeta">The metadata of the sender generating the diagnostic message.</param>
        /// <param name="category">The category used to classify the diagnostic message.</param>
        /// <param name="message">The content of the diagnostic message.</param>
        /// <param name="level">The severity level of the diagnostic message.</param>
        /// <returns>A new instance of <see cref="IShadowDiagnosticMessage"/>.</returns>
        public IShadowDiagnosticMessage Create(IShadowDiagnosableMeta senderMeta, string category, string message, ShadowDiagnosticLevel level)
        {
            return Create(senderMeta, category, message, level, DateTimeOffset.Now);
        }

        /// <summary>
        /// Creates a new <see cref="IShadowDiagnosticMessage"/> instance with the specified parameters.
        /// </summary>
        /// <param name="senderMeta">The metadata of the sender generating the diagnostic message.</param>
        /// <param name="category">The category used to classify the diagnostic message.</param>
        /// <param name="message">The content of the diagnostic message.</param>
        /// <returns>A new instance of <see cref="IShadowDiagnosticMessage"/>.</returns>
        public IShadowDiagnosticMessage Create(IShadowDiagnosableMeta senderMeta, string category, string message)
        {
            return Create(senderMeta, category, message, ShadowDiagnosticLevel.Info);
        }
    }
}
