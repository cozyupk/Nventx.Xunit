using System.Diagnostics;
using Cozyupk.HelloShadowDI.ComponentMeta.Utils.Contracts;

/// <summary>
/// A default diagnostic observer that writes diagnostic messages to the debug output.
/// This implementation formats messages with a timestamp and severity level prefix.
/// It is only active in Debug builds; no output will be generated in Release builds.
/// </summary>
public class DefaultDiagnosticObserver : IShadowDiagnosticObserver
{
    /// <summary>
    /// Handles a diagnostic message by formatting it with a timestamp and severity level,
    /// and outputs it to the debug output. This is only active in Debug builds.
    /// </summary>
    public void OnDiagnostic(IShadowDiagnosticMessage message)
    {
        // Outputs the diagnostic message to the debug output.
        // This is only active in Debug builds; no output will be generated in Release builds.
        var prefix = GetPrefix(message.Level);
        var formatted = $"{prefix} [{message.Timestamp:yyyy-MM-dd HH:mm:ss.fff}] {message.Message}";
        Debug.WriteLine(formatted);
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