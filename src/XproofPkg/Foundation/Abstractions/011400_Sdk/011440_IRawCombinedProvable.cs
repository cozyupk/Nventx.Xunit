using System.Collections.Generic;
using System.Reflection;

namespace Xproof.Abstractions.Sdk
{
    /// <summary>
    /// An interface that defines a provable element that can be probed for failures.
    /// </summary>
    public interface IRawCombinedProvable<in TLabelAxes>
    {
        /// <summary>
        /// Probes the provable element for failures, optionally with a label and caller information.
        /// </summary>
        void Probe(
            TLabelAxes? label,
            string callerFilePath,
            int callerLineNumber,
            string callerMemberName,
            MethodInfo invokedMethodInfo,
            object?[] invokedParameters
        );
    }

    /// <summary>
    /// An interface that defines a provable element that can be probed for results of type T.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRawCombinedProvable<out T, in TLabelAxes>
    {
        /// <summary>
        /// Probes the provable element for results of type T, optionally with a label and caller information.
        /// </summary>
        IEnumerable<T?> Probe(
            TLabelAxes? label,
            string callerFilePath,
            int callerLineNumber,
            string callerMemberName,
            MethodInfo invokedMethodInfo,
            object?[] invokedParameters
        );
    }
}
