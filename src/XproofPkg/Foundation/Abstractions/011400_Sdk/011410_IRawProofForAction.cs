using System;
using System.Reflection;
using Xproof.Abstractions.TestProofForTestRunner;

namespace Xproof.Abstractions.Sdk
{
    public interface IRawProofForAction<in TAxes>
    {
        public abstract void Probe(
            Action act,
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
