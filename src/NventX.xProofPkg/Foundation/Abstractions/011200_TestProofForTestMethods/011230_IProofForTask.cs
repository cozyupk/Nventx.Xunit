using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace NventX.xProof.Abstractions.TestProofForTestMethods
{
    public interface IProofForTask
    {
        Task ProbeAsync(
            Func<Task> task,
            string? label,
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string? callerMemberName = null
        );
    }
}
