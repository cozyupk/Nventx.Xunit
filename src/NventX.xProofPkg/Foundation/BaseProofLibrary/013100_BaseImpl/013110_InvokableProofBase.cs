using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using NventX.xProof.Abstractions.TestProofForTestRunner;
using NventX.xProof.Utils;

namespace NventX.xProof.BaseProofLibrary.BaseImpl
{
    /// <summary>
    /// Base class for invokable proofs, providing setup and failure collection functionality.
    /// </summary>
    public abstract class InvokableProofBase : IInvokableProof
    {
        /// <summary>
        /// The kind of proof invocation, indicating whether it is a single case, parameterized, or unknown.
        /// </summary>
        protected internal ProofInvocationKind ProofInvocationKind { get; private set; }

        /// <summary>
        /// A collection to store probing failures that occur during the test execution.
        /// </summary>
        protected ConcurrentQueue<IProbingFailure> ProbingFailures { get; } = new();

        /// <summary>
        /// Sets up the test proof environment with the specified proof invocation kind.
        /// </summary>
        public void Setup(ProofInvocationKind proofInvocationKind)
        {
            // Validate the proof invocation kind and set it
            if (proofInvocationKind != ProofInvocationKind.SingleCase
                && proofInvocationKind != ProofInvocationKind.Parameterized
                && proofInvocationKind != ProofInvocationKind.Unknown)
            {
                throw new System.ArgumentException(
                    $"Invalid proof invocation kind: {proofInvocationKind}. " +
                    "Must be one of SingleCase, Parameterized, or Unknown.",
                    nameof(proofInvocationKind));
            }
            ProofInvocationKind = proofInvocationKind;
        }

        /// <summary>
        /// Collects probing failures encountered during the test execution.
        /// </summary>
        public IEnumerable<IProbingFailure> CollectProbingFailure()
        {
            // Return a copy of the probing failures to avoid modification during enumeration
            return ProbingFailures.ToArray();
        }

        /// <summary>
        /// Records a probing failure with a label, exception and caller details.
        /// </summary>
        public void RecordProbingFailure(
            string? label, Delegate act, Exception ex,
            string? callerFilePath, int callerLineNumber, string? callerMemberName
        )
        {
            var location = $"{Path.GetFileName(callerFilePath)}:{callerLineNumber}";
            var method = act.Method;
            var delegateInfo = $"{method.DeclaringType?.Name}.{method.Name}";

            // Combine user label (if any) with contextual info
            var effectiveLabel = string.IsNullOrWhiteSpace(label)
                ? $"{location} ({callerMemberName}) → {delegateInfo}"
                : $"{label} @ {location} ({callerMemberName})";

            // Record the probing failure with the label and exception
            ProbingFailures.Enqueue(new ProbingFailure(effectiveLabel, ex));
        }
    }
}
