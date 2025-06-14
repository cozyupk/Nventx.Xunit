using System;

namespace Xproof.Abstractions.TestProofForTestRunner
{
    /// <summary>
    /// Represents a scope for probing operations in the context of test proofs.
    /// </summary>
    public interface IProbeScope : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// Records a successful probing operation, indicating that the operation completed without issues.
        /// </summary>
        void RecordSuccess();

        /// <summary>
        /// Records a failure during the probing operation, capturing the exception that occurred.
        /// </summary>
        /// <param name="ex"></param>
        void RecordFailure(Exception ex);
    }
}
