using System;
using System.Reflection;

namespace Xproof.Abstractions.Sdk
{
    public interface IRawProofForAction
    {
        public abstract void Probe(
            Action act,
            object? axes,
            string callerFilePath,
            int callerLineNumber,
            string callerMemberName,
            MethodInfo invokedMethodInfo,
            object?[] invokedParameters
        );
    }
}
