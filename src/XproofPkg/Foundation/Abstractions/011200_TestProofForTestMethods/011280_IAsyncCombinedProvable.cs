using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Xproof.Abstractions.TestProofForTestMethods
{
    public interface IAsyncCombinedProvable<in TLabelAxes>
    {
        Task ProbeAsync(
            TLabelAxes? label = default,
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string? callerMemberName = null,
            MethodInfo? invokedMethodInfo = null,
            object?[]? invokedParameters = null
        );
    }
}
