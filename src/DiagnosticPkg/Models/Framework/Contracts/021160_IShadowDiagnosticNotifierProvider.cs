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
        /// <param name="sender">The object requesting the notifier, or null if not applicable.</param>
        /// <param name="category">The diagnostic category or prefix for the notifier.</param>
        /// <returns>An instance of <see cref="IShadowDiagnosticNotifier"/>.</returns>
        IShadowDiagnosticNotifier CreateDiagnosticNotifier(object? sender, string category);
    }
}
