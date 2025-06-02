using NventX.xProof.Abstractions;
using Xunit.Abstractions;

namespace NventX.Xunit
{
    /// <summary>
    /// A discoverer for test cases that expect a proof to be verified during their execution.
    /// </summary>
    public class TheoryTestCaseDiscoverer<TTestProof> : TestCaseForProofDiscoverer<TTestProof>
        where TTestProof : ITestProof
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TheoryTestCaseDiscoverer{TTestProof}"/> class with the specified diagnostic message sink.
        /// </summary>
        public TheoryTestCaseDiscoverer(IMessageSink diagnosticMessageSink) : base(ProofInvocationKind.Parameterized, diagnosticMessageSink)
        {
            // No additional initialization is needed for FactTestCaseDiscoverer
        }
    }
}
