using NventX.xProof.Abstractions;
using Xunit.Sdk;

namespace NventX.Xunit
{
    /// <summary>
    /// Represents a test case that expects a proof to be verified during its execution.
    /// </summary>
    internal interface ITestCaseForProof : IXunitTestCase
    {
        /// <summary>
        /// Creates an instance of the test proof associated with this test case.
        /// </summary>
        /// <returns></returns>
        ITestProof CreateTestProof();

        /// <summary>
        /// Gets the invocation kind of the proof for this test case.
        /// </summary>
        public ProofInvocationKind ProofInvocationKind { get; }
    }
}
