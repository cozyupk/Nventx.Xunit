using System;
using System.Reflection;

namespace Xproof.Abstractions.Sdk
{
    public interface IRawProofForFunc
    {
        T? Probe<T> (
            Func<T> func,
            object? axes,
            string callerFilePath,
            int callerLineNumber,
            string callerMemberName,
            MethodInfo invokedMethodInfo,
            object?[] invokedParameters
        );
    }
}
