using System.Reflection;
using System.Threading.Tasks;

namespace Xproof.Abstractions.Sdk
{
    public interface IAsyncCombinedProvable<in TAxes>
    {
        Task ProbeAsync(
            TAxes? axes,
            string callerFilePath,
            int callerLineNumber,
            string callerMemberName,
            MethodInfo invokedMethodInfo,
            object?[] invokedParameters
        );
    }
}
