using System;
using NventX.xProof.Abstractions.TestProofForTestRunner;

namespace NventX.xProof.Utils
{
    /// <summary>
    /// Represents a failure encountered during test probing.
    /// </summary>
    public class ProbingFailure : IProbingFailure
    {
        /// <summary>
        /// Human-readable description of the failure.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// The underlying exception that caused the failure.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProbingFailure"/> class with a message and an exception.
        /// </summary>
        public ProbingFailure(string message, Exception exception)
        {
            Message = message;
            Exception = exception;
        }
    }
}
