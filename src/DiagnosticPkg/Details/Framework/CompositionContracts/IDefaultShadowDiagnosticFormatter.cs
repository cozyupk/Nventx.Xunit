using Cozyupk.Shadow.Flow.DiagnosticPkg.Models.Framework.Contracts;

namespace Cozyupk.Shadow.Flow.DiagnosticPkg.Details.Framework.CompositionContracts
{
    /// <summary>
    /// Provides a default implementation contract for formatting shadow diagnostic messages into string output.
    /// </summary>
    public interface IDefaultShadowDiagnosticFormatter : IShadowDiagnosticFormatter<string>
    {
    }
}
