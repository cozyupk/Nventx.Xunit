using Cozyupk.Shadow.Flow.DiagnosticPkg.Details.Framework.CompositionContracts;
using Cozyupk.Shadow.Flow.DiagnosticPkg.Models.Framework.Contracts;

namespace Cozyupk.Shadow.Flow.DiagnosticPkg.Details.Framework.Impl
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
            // Get the prefix based on the message level
            var prefix = GetPrefix(message.Level);

            // Get sender name
            var senderName = message.SenderMeta.Label;

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
