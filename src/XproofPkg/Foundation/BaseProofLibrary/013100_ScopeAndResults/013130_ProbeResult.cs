using System;
using Xproof.Abstractions.TestProofForTestRunner;

namespace Xproof.BaseProofLibrary.ScopeAndResults
{
    /// <summary>
    /// Represents the result of a probing operation in the context of test proofs.
    /// </summary>
    public record ProbeResult<TLabelAxes> : IProbeResult<TLabelAxes>
    {
        /// <summary>
        /// The record of the invocation that was used to perform the probing.
        /// </summary>
        public IProbeScopeRecord<TLabelAxes> ProbeScopeRecord { get; }

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
            IProbeScopeRecord<TLabelAxes> invocationRecord,
            TimeSpan elapsed,
            Exception? exception = null
        )
        {
            ProbeScopeRecord = invocationRecord ?? throw new ArgumentNullException(nameof(invocationRecord));
            Elapsed = elapsed;
            Exception = exception;
        }

        /// <summary>
        /// Returns a string representation of the probe result, indicating whether it was successful or encountered an exception.
        /// </summary>
        public override string ToString()
        {
            return $"[{(Exception == null ? "Very True" : "Oops!")}] {ProbeScopeRecord}";
        }
    }
}
