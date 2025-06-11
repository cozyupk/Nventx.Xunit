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
        protected internal ProofInvocationKind? ProofInvocationKind { get; private set; }

        /// <summary>
        /// Lock object to ensure thread-safe access to the proof invocation kind.
        /// </summary>
        private object SetupLock { get; } = new object();

        /// <summary>
        /// Lock object to ensure thread-safe incrementing of the success count.
        /// </summary>
        private object IncrementSuccessCountLock { get; } = new object();

        /// <summary>
        /// A collection to store probing failures that occur during the test execution.
        /// </summary>
        protected ConcurrentQueue<IProbingFailure> ProbingFailures { get; } = new();

        /// <summary>
        /// The count of successful probing attempts during the test execution.
        /// </summary>
        public int ProbingSuccessCount { get; private set; } = 0;

        /// <summary>
        /// Sets up the test proof environment with the specified proof invocation kind.
        /// </summary>
        public void Setup(ProofInvocationKind proofInvocationKind)
        {
            lock (SetupLock)
            {
                // If the proof invocation kind is already set, throw an exception
                if (ProofInvocationKind.HasValue)
                {
                    throw new System.InvalidOperationException(
                        "Setup has already been called. Cannot change the proof invocation kind after setup.");
                }

                // Validate the proof invocation kind and set it
                if (!Enum.IsDefined(typeof(ProofInvocationKind), proofInvocationKind))
                {
                    throw new System.ArgumentException(
                        $"Invalid proof invocation kind: {proofInvocationKind}. " +
                        "Must be one of SingleCase, Parameterized, or Unknown.",
                        nameof(proofInvocationKind));
                }
                ProofInvocationKind = proofInvocationKind;
            }
        }

        /// <summary>
        /// Collects probing failures encountered during the test execution.
        /// </summary>
        public IEnumerable<IProbingFailure> CollectProbingFailure()
        {
            // Return a copy of the probing failures so that we can avoid modification during enumeration
            return ProbingFailures.ToArray();
        }

        /// <summary>
        /// Records a probing failure with a label, exception and caller details.
        /// </summary>
        public void RecordProbingFailure(
            string? label, Delegate act, Exception ex,
            string? callerFilePath, int callerLineNumber, string? callerMemberName,
            int cnt, int totalCnt
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

        /// <summary>
        /// Records a successful probing attempt, incrementing the success count in a thread-safe manner.
        /// </summary>
        public void RecordProobingSuccess()
        {
            // Increment the success count in a thread-safe manner
            lock (IncrementSuccessCountLock)
            {
                ++ProbingSuccessCount;
            }
        }
    }
}
