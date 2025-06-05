using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace NventX.xProof.Abstractions.TestProofForTestMethods
{
    public interface IAsyncProvable
    {
        Task ProbeAsync(
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string? callerMemberName = null
        );
    }
}
