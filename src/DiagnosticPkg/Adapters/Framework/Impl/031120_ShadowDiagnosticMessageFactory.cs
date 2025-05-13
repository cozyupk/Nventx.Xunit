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
        /// <param name="sender">The source object of the diagnostic message (can be null).</param>
        /// <param name="category">The category of the diagnostic message.</param>
        /// <param name="message">The content of the diagnostic message.</param>
        /// <param name="level">The severity level of the diagnostic message. Defaults to Info.</param>
        /// <param name="timestamp">The timestamp of the diagnostic message. If null, the current time is used.</param>
        /// <returns>A new instance of <see cref="IShadowDiagnosticMessage"/>.</returns>
        public IShadowDiagnosticMessage Create(
            object? sender,
            string category,
            string message,
            ShadowDiagnosticLevel level,
            DateTimeOffset timestamp)
        {
            // Create and return a new diagnostic message with the provided parameters.
            return new ShadowDiagnosticMessage(sender, category, message, level, timestamp);
        }

        /// <summary>
        /// Creates a new <see cref="IShadowDiagnosticMessage"/> instance with the specified parameters.
        /// </summary>
        /// <param name="sender">The source object of the diagnostic message (can be null).</param>
        /// <param name="category">The category of the diagnostic message.</param>
        /// <param name="message">The content of the diagnostic message.</param>
        /// <param name="level">The severity level of the diagnostic message. Defaults to Info.</param>
        /// <returns>A new instance of <see cref="IShadowDiagnosticMessage"/>.</returns>
        public IShadowDiagnosticMessage Create(object? sender, string category, string message, ShadowDiagnosticLevel level)
        {
            return Create(sender, category, message, level, DateTimeOffset.Now);
        }

        /// <summary>
        /// Creates a new <see cref="IShadowDiagnosticMessage"/> instance with the specified parameters.
        /// </summary>
        /// <param name="sender">The source object of the diagnostic message (can be null).</param>
        /// <param name="category">The category of the diagnostic message.</param>
        /// <param name="message">The content of the diagnostic message.</param>
        /// <param name="level">The severity level of the diagnostic message. Defaults to Info.</param>
        /// <param name="timestamp">The timestamp of the diagnostic message. If null, the current time is used.</param>
        /// <returns>A new instance of <see cref="IShadowDiagnosticMessage"/>.</returns>
        public IShadowDiagnosticMessage Create(object? sender, string category, string message)
        {
            return Create(sender, category, message, ShadowDiagnosticLevel.Info);
        }
    }
}
