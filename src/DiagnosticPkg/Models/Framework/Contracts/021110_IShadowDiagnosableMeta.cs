namespace Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Contracts
{
    /// <summary>
    /// Represents metadata for diagnosable shadow objects.
    /// </summary>
    public interface IShadowDiagnosableMeta
    {
        /// <summary>
        /// Gets a human-readable label representing this diagnosable component.
        /// </summary>
        string Label { get; }
    }
}
