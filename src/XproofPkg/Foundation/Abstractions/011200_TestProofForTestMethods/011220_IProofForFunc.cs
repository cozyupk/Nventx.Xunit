using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xproof.Abstractions.TestProofForTestRunner;

namespace Xproof.Abstractions.TestProofForTestMethods
{
    public interface IProofForFunc<in TAxes>
    {
        T? Probe<T>
        (
            Func<T> func,
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
