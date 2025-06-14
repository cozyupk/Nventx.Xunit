using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Xproof.Abstractions.TestProofForTestMethods
{
    /// <summary>
    /// An interface that defines a provable element that can be probed for failures.
    /// </summary>
    public interface ICombinedProvable
    {
        /// <summary>
        /// Probes the provable element for failures, optionally with a label and caller information.
        /// </summary>
        void Probe(
            object? axes = null,
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string? callerMemberName = null,
            MethodInfo? invokedMethodInfo = null,
            object?[]? invokedParameters = null
        );
    }

    /// <summary>
    /// An interface that defines a provable element that can be probed for results of type T.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IProvable<out T>
    {
        /// <summary>
        /// Probes the provable element for results of type T, optionally with a label and caller information.
        /// </summary>
        IEnumerable<T?> Probe(
            object? axes = null,
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string? callerMemberName = null,
            MethodInfo? invokedMethodInfo = null,
            object?[]? invokedParameters = null
        );
    }
}
