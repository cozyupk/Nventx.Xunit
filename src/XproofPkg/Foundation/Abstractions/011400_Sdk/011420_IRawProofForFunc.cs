using System;
using System.Reflection;
using Xproof.Abstractions.TestProofForTestRunner;

namespace Xproof.Abstractions.Sdk
{
    public interface IRawProofForFunc<in TAxes>
    {
        T? Probe<T> (
            Func<T> func,
            TAxes? axes,
            string callerFilePath,
            int callerLineNumber,
            string callerMemberName,
            MethodInfo invokedMethodInfo,
            object?[] invokedParameters,
            IPositionInArray? combinedPosition
        );
    }
}
