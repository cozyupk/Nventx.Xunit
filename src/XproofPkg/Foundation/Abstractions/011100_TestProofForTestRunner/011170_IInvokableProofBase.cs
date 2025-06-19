using System.Collections.Generic;

namespace Xproof.Abstractions.TestProofForTestRunner
{
    public interface IInvokableProofBase
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
        /// Collects the test proof results without labels after the test execution is complete.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IProbeResultBase> GetResultBases();

        /// <summary>
        /// Commits the test proof results after the test execution is complete.
        /// </summary>
        void Commit();
    }
}