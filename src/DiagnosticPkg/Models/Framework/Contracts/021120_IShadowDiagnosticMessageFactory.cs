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
        /// <param name="sender">The source of the message (can be null).</param>
        /// <param name="category">The message category (e.g. component, layer, etc).</param>
        /// <param name="message">The diagnostic content.</param>
        /// <param name="level">The severity level.</param>
        /// <param name="timestamp">The  timestamp.</param>
        /// <returns>A diagnostic message instance.</returns>
        IShadowDiagnosticMessage Create(
            object? sender,
            string category,
            string message,
            ShadowDiagnosticLevel level,
            DateTimeOffset timestamp
        );

        /// <summary>
        /// Creates a new diagnostic message instance.
        /// </summary>
        /// <param name="sender">The source of the message (can be null).</param>
        /// <param name="category">The message category (e.g. component, layer, etc).</param>
        /// <param name="message">The diagnostic content.</param>
        /// <param name="level">The severity level.</param>
        /// <returns>A diagnostic message instance.</returns>
        IShadowDiagnosticMessage Create(
            object? sender,
            string category,
            string message,
            ShadowDiagnosticLevel level
        );

        /// <summary>
        /// Creates a new diagnostic message instance.
        /// </summary>
        /// <param name="sender">The source of the message (can be null).</param>
        /// <param name="category">The message category (e.g. component, layer, etc).</param>
        /// <param name="message">The diagnostic content.</param>
        /// <returns>A diagnostic message instance.</returns>
        IShadowDiagnosticMessage Create(
            object? sender,
            string category,
            string message
        );
    }
}
