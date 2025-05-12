namespace Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Contracts
{
    /// <summary>
    /// Provides a mechanism to create instances of <see cref="IShadowDiagnosticNotifier"/>.
    /// </summary>
    public interface IShadowDiagnosticNotifierProvider
    {
        /// <summary>
        /// Creates a diagnostic notifier with the specified prefix.
        /// </summary>
        /// <param name="prefix">The prefix to be used by the diagnostic notifier.</param>
        /// <returns>An instance of <see cref="IShadowDiagnosticNotifier"/>.</returns>
        IShadowDiagnosticNotifier CreateDiagnosticNotifier(string prefix);
    }
}
