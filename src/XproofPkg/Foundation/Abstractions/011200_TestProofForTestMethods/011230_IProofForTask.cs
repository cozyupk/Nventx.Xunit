using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xproof.Abstractions.TestProofForTestRunner;

namespace Xproof.Abstractions.TestProofForTestMethods
{
    public interface IProofForTask
    {
        Task ProbeAsync(
            Func<Task> task,
            object? axes = null,
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string? callerMemberName = null,
            MethodInfo? invokedMethodInfo = null,
            object?[]? invokedParameters = null,
            IPositionInArray? combinedPosition = null
        );
    }
}
