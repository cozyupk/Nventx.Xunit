using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Xproof.Abstractions.TestProofForTestMethods
{
    public interface IProofForAction
    {
        public abstract void Probe(
            Action act,
            object? axes = null,
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string? callerMemberName = null,
            MethodInfo? invokedMethodInfo = null,
            object?[]? invokedParameters = null,
            (int Index, int TotalCount)? combinedPosition = null
        );
    }
}
