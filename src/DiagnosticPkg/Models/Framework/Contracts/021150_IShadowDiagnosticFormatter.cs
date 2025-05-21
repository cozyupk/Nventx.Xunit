namespace Cozyupk.Shadow.Flow.DiagnosticPkg.Models.Framework.Contracts
{
    /// <summary>
    /// Defines a formatter that converts diagnostic messages into a specified output format.
    /// </summary>
    public interface IShadowDiagnosticFormatter<out TOutputFormat>
    {
        /// <summary>
        /// Formats the given diagnostic message into the specified output format.
        /// </summary>
        /// <param name="message">The diagnostic message to format.</param>
        /// <returns>The formatted output.</returns>
        TOutputFormat Format(IShadowDiagnosticMessage message);
    }
}
