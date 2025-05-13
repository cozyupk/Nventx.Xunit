using Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.Contracts;

namespace Cozyupk.HelloShadowDI.DiagnosticPkg.Models.Framework.CompositionContracts
{
    /// <summary>
    /// Defines the contract for the default shadow diagnostic observer.
    /// This interface is used by implementations that observe and handle standard diagnostic events.
    /// </summary>
    public interface IDefaultShadowDiagnosticObserver : IShadowDiagnosticObserver
    {
    }
}
