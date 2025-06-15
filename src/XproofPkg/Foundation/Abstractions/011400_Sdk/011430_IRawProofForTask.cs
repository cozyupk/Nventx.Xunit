using System;
using System.Reflection;
using System.Threading.Tasks;
using Xproof.Abstractions.TestProofForTestRunner;

namespace Xproof.Abstractions.Sdk
{
    public interface IRawProofForTask<in TAxes>
    {
        Task ProbeAsync(
            Func<Task> task,
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
