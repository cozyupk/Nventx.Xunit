using System.Reflection;
using System.Threading.Tasks;

namespace Xproof.Abstractions.Sdk
{
    public interface IAsyncCombinedProvable<in TLabelAxes>
    {
        Task ProbeAsync(
            TLabelAxes? label,
            string callerFilePath,
            int callerLineNumber,
            string callerMemberName,
            MethodInfo invokedMethodInfo,
            object?[] invokedParameters
        );
    }
}
