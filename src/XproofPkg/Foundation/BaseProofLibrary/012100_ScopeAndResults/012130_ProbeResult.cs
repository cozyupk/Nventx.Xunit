using System;
using System.Security.Cryptography;
using Xproof.Abstractions.TestProofForTestRunner;

namespace Xproof.BaseProofLibrary.ScopeAndResults
{
    /// <summary>
    /// Represents the result of a probing operation in the context of test proofs.
    /// </summary>
    public record ProbeResult<TAxes> : IProbeResult<TAxes>
    {
        /// <summary>
        /// The record of the invocation that was used to perform the probing.
        /// </summary>
        public IProbeScopeRecord<TAxes> ProbeScopeRecord { get; }

        /// <summary>
        /// The time elapsed during the probing operation.
        /// </summary>
        public TimeSpan Elapsed { get; }

        /// <summary>
        /// The exception that occurred during the probing operation, if any.
        /// </summary>
        public Exception? Exception { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProbeResult"/> class with the specified parameters.
        /// </summary>
        public ProbeResult(
            IProbeScopeRecord<TAxes> invocationRecord,
            TimeSpan elapsed,
            Exception? exception = null
        )
        {
            ProbeScopeRecord = invocationRecord ?? throw new ArgumentNullException(nameof(invocationRecord));
            Elapsed = elapsed;
            Exception = exception;
        }
    }
}
