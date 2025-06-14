using System;
using System.Reflection;
using Xproof.Abstractions.TestProofForTestRunner;

namespace Xproof.BaseProofLibrary.ScopeAndResults
{
    public class ProbeScopeRecord : IProbeScopeRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProbeScopeRecord"/> class with the specified parameters.
        /// </summary>
        public MethodInfo InvokedProbeMethod { get; }

        /// <summary>
        /// The parameters that were passed to the test method during the probing invocation.
        /// </summary>
        public object?[] InvocationParameters { get; }

        /// <summary>
        /// The way the probing was invoked, indicating whether it was a single case, parameterized test, or unknown.
        /// </summary>
        public ProofInvocationKind InvocationKind { get; }

        /// <summary>
        /// The file path of the source file where the probing was invoked.
        /// </summary>
        public string CallerFilePath { get; }

        /// <summary>
        /// The line number in the source file where the probing was invoked.
        /// </summary>
        public int CallerLineNumber { get; }

        /// <summary>
        /// The name of the method that invoked the probing.
        /// </summary>
        public string CallerMemberName { get; }

        /// <summary>
        /// The label associated with the probe, if any.
        /// </summary>
        public object? Axes { get; }

        /// <summary>
        /// The position of the probe in a sequence, if applicable.
        /// </summary>
        public (int Index, int TotalCount)? Position { get; }

        public ProbeScopeRecord(
            ProofInvocationKind invocationKind,
            MethodInfo invokedProbeMethod,
            object?[] invocationParameters,
            string callerFilePath,
            int callerLineNumber,
            string callerMemberName,
            object? axes,
            (int Index, int TotalCount)? position = null
        )
        {
            InvokedProbeMethod = invokedProbeMethod ?? throw new ArgumentNullException(nameof(invokedProbeMethod));
            InvocationParameters = invocationParameters ?? throw new ArgumentNullException(nameof(invocationParameters));
            InvocationKind = invocationKind;
            CallerFilePath = callerFilePath ?? throw new ArgumentNullException(nameof(callerFilePath));
            CallerLineNumber = callerLineNumber;
            CallerMemberName = callerMemberName ?? throw new ArgumentNullException(nameof(callerMemberName));
            Axes = axes;
            Position = position;
        }
    }
}
