using System;
using System.Collections.Generic;

namespace Cozyupk.HelloShadowDI.ComponentMeta.Utils.Contracts
{
    public interface IShadowDiagnosticNotifier
    {
        void Notify(string message, ShadowDiagnosticLevel level = ShadowDiagnosticLevel.Info);
        void NotifyIfObserved(Func<List<string>> messageFactory, ShadowDiagnosticLevel level = ShadowDiagnosticLevel.Info);
    }
}
