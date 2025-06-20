using System;
using Xproof.Abstractions.TestProofForTestMethods;

namespace Xproof.ProbeCore.ProbeOutcome
{
    /// <summary>
    /// Represents a failed probe outcome, storing an exception that will be thrown when unwrapped.
    /// This allows deferred exception handling during probe evaluation.
    /// </summary>
    public class ProbeExceptionOutcome<T> : IProbeOutcome<T>
    {
        /// <summary>
        /// Stores the exception that occurred during the probe operation.
        /// </summary>
        protected Exception Exception { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProbeExceptionOutcome{T}"/> class with the specified exception.
        /// </summary>
        public ProbeExceptionOutcome(Exception exception)
        {
            Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        }
    }

    /// <summary>
    /// Represents a failed probe outcome that can be unwrapped to throw the stored exception.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ProbeExceptionOutcomeInternal<T> : ProbeExceptionOutcome<T>, IProbeOutcomeInternal<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProbeExceptionOutcomeInternal{T}"/> class with the specified exception.
        /// </summary>
        public ProbeExceptionOutcomeInternal(Exception exception) : base(exception)
        {
            // No additional initialization needed.
        }

        /// <summary>
        /// Unwraps the outcome, throwing the stored exception.
        /// </summary>
        /// <returns>Never returns; always throws the stored exception.</returns>
        public T Unwrap() => throw Exception;
    }
}
