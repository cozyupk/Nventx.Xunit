namespace Cozyupk.HelloShadowDI.ComponentMeta.Utils.Contracts
{
    public interface IShadowDiagnosticNotifierProvider
    {
        IShadowDiagnosticNotifier CreateDiagnosticNotifier(string prefix);
    }
}
