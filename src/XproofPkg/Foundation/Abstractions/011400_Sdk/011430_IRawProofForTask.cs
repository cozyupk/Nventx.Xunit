using System;
using System.Threading.Tasks;

namespace Xproof.Abstractions.Sdk
{
    public interface IRawProofForTask
    {
        Task ProbeAsync(
            Func<Task> task,
            object? axes,
            string callerFilePath,
            int callerLineNumber,
            string callerMemberName
        );
    }
}
