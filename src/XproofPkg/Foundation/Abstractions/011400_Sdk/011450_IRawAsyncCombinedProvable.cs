using System.Reflection;
using System.Threading.Tasks;

namespace Xproof.Abstractions.Sdk
{
    public interface IAsyncCombinedProvable
    {
        Task ProbeAsync(
            object? axes,
            string callerFilePath,
            int callerLineNumber,
            string callerMemberName,
            MethodInfo invokedMethodInfo,
            object?[] invokedParameters
        );
    }
}
