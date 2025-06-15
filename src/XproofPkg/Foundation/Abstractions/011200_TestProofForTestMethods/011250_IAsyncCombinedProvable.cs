using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Xproof.Abstractions.TestProofForTestMethods
{
    public interface IAsyncCombinedProvable<in TAxes>
    {
        Task ProbeAsync(
            TAxes? axes = default,
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string? callerMemberName = null,
            MethodInfo? invokedMethodInfo = null,
            object?[]? invokedParameters = null
        );
    }
}
