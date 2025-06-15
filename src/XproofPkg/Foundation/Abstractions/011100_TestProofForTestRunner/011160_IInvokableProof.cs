using System.Collections.Generic;

namespace Xproof.Abstractions.TestProofForTestRunner
{
    /// <summary>
    /// Interface for test proof, which is used to set up and collect probing failures.
    /// </summary>
    public interface IInvokableProof<TAxes>
    {
        /// <summary>
        /// Sets up the test proof environment.
        /// </summary>
        void Setup(ProofInvocationKind proofInvocationKind);

        /// <summary>
        /// Gets the kind of proof invocation, indicating whether it is a single case, parameterized, or unknown.
        /// </summary>
        ProofInvocationKind ProofInvocationKind { get; }

        /// <summary>
        /// Collects probing failures encountered during the test execution.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IProbeResult<TAxes>> GetResults();

        /// <summary>
        /// Records a probe result from the test execution, 
        /// which can be a failure or success. And if the result is a success, Exception Property should be null.
        /// </summary>
        void RecordProbeResult(
            IProbeResult<TAxes> probeResult
        );

        /// <summary>
        /// Commits the test proof results after the test execution is complete.
        /// </summary>
        void Commit();
    }
}
