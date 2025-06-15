using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xproof.Abstractions.TestProofForTestRunner;

namespace Xproof.Abstractions.TestProofForTestMethods
{
    public interface IProofForTask<in TAxes>
    {
        Task ProbeAsync(
            Func<Task> task,
            TAxes? axes = default,
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string? callerMemberName = null,
            MethodInfo? invokedMethodInfo = null,
            object?[]? invokedParameters = null,
            IPositionInArray? combinedPosition = null
        );
    }
}
