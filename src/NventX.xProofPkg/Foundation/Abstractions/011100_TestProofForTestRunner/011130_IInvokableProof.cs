using System;
using System.Collections.Generic;

namespace NventX.xProof.Abstractions.TestProofForTestRunner
{
    /// <summary>
    /// Interface for test proof, which is used to set up and collect probing failures.
    /// </summary>
    public interface IInvokableProof
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

        /// <summary>
        /// Records a probing failure with a label, exception and caller details.
        /// </summary>
        void RecordProbingFailure(
            string? label, Delegate act, Exception ex,
            string? callerFilePath, int callerLineNumber, string? callerMemberName
        );
    }
}
