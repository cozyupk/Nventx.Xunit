namespace Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Contracts
{
    /// <summary>
    /// Defines a contract for receiving diagnostic messages emitted during application execution.
    /// Implementers can use this interface to observe and process structural or runtime diagnostics.
    /// </summary>
    public interface IShadowDiagnosticObserver
    {
        /// <summary>
        /// Handles a diagnostic message emitted by the system.
        /// </summary>
        /// <param name="message">The diagnostic message describing the event or condition.</param>
        void OnDiagnostic(IShadowDiagnosticMessage message);
    }
}
