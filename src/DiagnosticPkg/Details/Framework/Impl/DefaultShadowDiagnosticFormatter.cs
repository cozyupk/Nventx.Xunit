using Cozyupk.HelloShadowDI.DiagnosticPkg.Details.Framework.CompositionContracts;
using Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Contracts;

namespace Cozyupk.HelloShadowDI.DiagnosticPkg.Details.Framework.Impl
{
    /// <summary>
    /// Default implementation of the diagnostic formatter that converts diagnostic messages
    /// into a standardized string representation for logging or display purposes.
    /// </summary>
    public class DefaultShadowDiagnosticFormatter : IDefaultShadowDiagnosticFormatter
    {
        /// <summary>
        /// Formats the given diagnostic message into a string representation.
        /// </summary>
        /// <param name="message">The diagnostic message to format.</param>
        /// <returns>A string representation of the diagnostic message.</returns>
        public string Format(IShadowDiagnosticMessage message)
        {
            // Get the string prefix for the diagnostic level (e.g., [INFO], [ERROR], etc.)
            var prefix = GetPrefix(message.Level);

            // Get the type of the sender object, if available
            var tSender = message.Sender?.GetType();

            // Get the simple type name of the sender, or "Unknown" if sender is null
            var senderName = tSender?.Name ?? "Unknown";

            // Get the string representation of the sender object
            var senderDetails = message.Sender?.ToString();

            // If sender details are meaningful and different from the type name/full name, append them
            if (senderDetails == senderName)
                senderDetails = null; // Clear senderDetails if it matches the type name
            if (senderDetails == tSender?.FullName)
                senderDetails = null; // Clear senderDetails if it matches the full name
            if (senderDetails != null)
                senderName = $"{senderName} ({senderDetails})";

            // Append the sender's hash code in hexadecimal if sender is not null
            if (message.Sender != null)
                senderName += $"/{message.Sender.GetHashCode():x}";

            // Format the final diagnostic message string with timestamp, category, sender, and message
            var formatted = $"{prefix} [{message.Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{message.Category}] ({senderName}) {message.Message}";

            return formatted;
        }

        /// <summary>
        /// Retrieves a string prefix corresponding to the specified diagnostic severity level.
        /// </summary>
        /// <param name="level">The severity level of the diagnostic message.</param>
        /// <returns>A string prefix representing the severity level.</returns>
        private static string GetPrefix(ShadowDiagnosticLevel level)
        {
            return level switch
            {
                ShadowDiagnosticLevel.Trace => "[TRACE]  ",
                ShadowDiagnosticLevel.Debug => "[DEBUG]  ",
                ShadowDiagnosticLevel.Info => "[INFO]   ",
                ShadowDiagnosticLevel.Notice => "[NOTICE] ",
                ShadowDiagnosticLevel.Warning => "[WARN]   ",
                ShadowDiagnosticLevel.Error => "[ERROR]  ",
                ShadowDiagnosticLevel.Critical => "[FATAL]  ",
                _ => "[DIAG]   "
            };
        }
    }
}
