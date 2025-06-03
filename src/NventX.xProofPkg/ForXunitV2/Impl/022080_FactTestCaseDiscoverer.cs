using NventX.xProof.Abstractions.TestProofForTestRunner;
using Xunit.Abstractions;

namespace NventX.xProof.Xunit
{
    /// <summary>
    /// A discoverer for test cases that expect a proof to be verified during their execution.
    /// </summary>
    public class FactTestCaseDiscoverer<TTestProof> : TestCaseForProofDiscoverer<TTestProof>
        where TTestProof : IInvokableProof
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FactTestCaseDiscoverer{TTestProof}"/> class with the specified diagnostic message sink.
        /// </summary>
        public FactTestCaseDiscoverer(IMessageSink diagnosticMessageSink) : base(ProofInvocationKind.SingleCase, diagnosticMessageSink)
        {
            // No additional initialization is needed for FactTestCaseDiscoverer
        }
    }
}
