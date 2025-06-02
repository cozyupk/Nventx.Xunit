using System.Collections.Generic;

namespace NventX.xProof.Abstractions
{
    /// <summary>
    /// Interface for test proof, which is used to set up and collect probing failures.
    /// </summary>
    public interface ITestProof
    {
        /// <summary>
        /// Sets up the test proof environment.
        /// </summary>
        void Setup(ProofInvocationKind proofInvocationKind);

        /// <summary>
        /// Collects probing failures encountered during the test execution.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IProbingFailure> CollectProbingFailure();
    }
}
