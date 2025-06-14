using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Xproof.Abstractions.TestProofForTestMethods
{
    public interface IProofForFunc
    {
        T? Probe<T>
        (
            Func<T> func,
            object? axes = null,
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string? callerMemberName = null,
            MethodInfo? invokedMethodInfo = null,
            object?[]? invokedParameters = null
        );
    }
}
