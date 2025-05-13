using System;

namespace Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Contracts
{
    /// <summary>
    /// Represents the severity level of a diagnostic message.
    /// </summary>
    public enum ShadowDiagnosticLevel
    {
        /// <summary>
        /// Extremely detailed logs for tracking internal structural flow.
        /// Ideal for recursive scans, internal state tracing, or instrumentation.
        /// </summary>
        Trace,

        /// <summary>
        /// Developer-intended debug messages to explain decisions or choices taken.
        /// Useful for analyzing injector behavior and decision paths.
        /// </summary>
        Debug,

        /// <summary>
        /// Standard informational messages indicating successful operations or neutral events.
        /// </summary>
        Info,

        /// <summary>
        /// Non-critical but noteworthy structural observations.
        /// May indicate ambiguities or edge cases, such as multiple inject candidates.
        /// </summary>
        Notice,

        /// <summary>
        /// Potentially harmful structural configurations or deprecated usage patterns.
        /// Application continues but structure may be fragile or suboptimal.
        /// </summary>
        Warning,

        /// <summary>
        /// Clearly invalid or broken configurations that prevent expected behavior.
        /// Examples include missing implementations or injection failures.
        /// </summary>
        Error,

        /// <summary>
        /// Critical failures that may compromise execution or violate architectural integrity.
        /// Likely requires immediate developer attention or halting execution.
        /// </summary>
        Critical
    }

    /// <summary>
    /// Provides details about a diagnostic event, including its content, severity level, 
    /// category, sender, and the time it occurred.
    /// </summary>
    public interface IShadowDiagnosticMessage
    {
        /// <summary>
        /// Gets the sender of the diagnostic message.
        /// This can be any object that is the source of the message.
        /// </summary>
        object? Sender { get; }

        /// <summary>
        /// Gets the category of the diagnostic message.
        /// Categories are used to group or classify messages for easier filtering or analysis.
        /// </summary>
        string Category { get; }

        /// <summary>
        /// Gets the severity level of the diagnostic message.
        /// </summary>
        ShadowDiagnosticLevel Level { get; }

        /// <summary>
        /// Gets the content of the diagnostic message.
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Gets the timestamp when the diagnostic message was created.
        /// </summary>
        DateTimeOffset Timestamp { get; }
    }
}
