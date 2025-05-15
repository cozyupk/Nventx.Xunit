using Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Contracts;

namespace Cozyupk.HelloShadowDI.DiagnosticPkg.Details.Framework.CompositionContracts
{
    /// <summary>
    /// Provides a default implementation contract for formatting shadow diagnostic messages into string output.
    /// </summary>
    public interface IDefaultShadowDiagnosticFormatter : IShadowDiagnosticFormatter<string>
    {
    }
}
