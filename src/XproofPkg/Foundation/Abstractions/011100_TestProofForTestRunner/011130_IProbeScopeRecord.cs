using System.Reflection;

namespace Xproof.Abstractions.TestProofForTestRunner
{
    /// <summary>
    /// Represents a result of probing in the context of test proofs.
    /// </summary>
    /// <remarks>
    /// Note: Expected / Actual values from assertions are not included here yet.
    /// These may be added in the future once a reliable way to extract them from xUnit's Assert results is established.
    /// </remarks>
    public interface IProbeScopeRecord<out TLabelAxes>
    {
        /// <summary>
        /// Invoked probe method that was used to perform the probing.
        /// </summary>
        MethodInfo InvokedProbeMethod { get; }

        /// <summary>
        /// The way the probing was invoked, indicating whether it was a single case, parameterized test, or unknown.
        /// </summary>
        ProofInvocationKind InvocationKind { get; }

        /// <summary>
        /// The parameters that were passed to the test method during the probing invocation.
        /// </summary>
        object?[] InvocationParameters { get; }

        /// <summary>
        /// The file path of the source file where the probing was invoked.
        /// </summary>
        string CallerFilePath { get; }

        /// <summary>
        /// The line number in the source file where the probing was invoked.
        /// </summary>
        int CallerLineNumber { get; }

        /// <summary>
        /// The name of the method that invoked the probing.
        /// </summary>
        string CallerMemberName { get; }

        /// <summary>
        /// The label associated with the probe, if any.
        /// </summary>
        TLabelAxes? label { get; }

        /// <summary>
        /// The position of the probe in a combined sequence, if applicable.
        /// </summary>
        IPositionInArray? CombinedPosition { get; }

        /// <summary>
        /// The position of the probe in a delegate sequence, if applicable.
        /// </summary>
        IPositionInArray? DelegatePosition { get; }
    }
}
