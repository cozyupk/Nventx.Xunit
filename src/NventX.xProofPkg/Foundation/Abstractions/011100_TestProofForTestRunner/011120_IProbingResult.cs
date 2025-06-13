using System;

namespace NventX.xProof.Abstractions.TestProofForTestRunner
{
    /// <summary>
    /// Represents a failure encountered during test probing.
    /// </summary>
    public interface IProbingResult
    {
        /// <summary>
        /// Human-readable description of the failure.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// The underlying exception that caused the failure.
        /// </summary>
        public Exception Exception { get; }
    }
}
