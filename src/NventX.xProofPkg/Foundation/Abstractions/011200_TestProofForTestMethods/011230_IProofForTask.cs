using System;
using System.Threading.Tasks;

namespace NventX.xProof.Abstractions.TestProofForTestMethods
{
    public interface IProofForTask
    {
        Task ProbeAsync(
            Func<Task> task,
            string? label = null,
            string? callerFilePath = null,
            int callerLineNumber = 0,
            string? callerMemberName = null
        );
    }
}
