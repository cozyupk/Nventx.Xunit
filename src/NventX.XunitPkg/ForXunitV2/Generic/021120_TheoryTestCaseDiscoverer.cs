using Xunit.Abstractions;

namespace NventX.Xunit.Generic
{
    public class TheoryTestCaseDiscoverer<TTestProof> : TestCaseForProofDiscoverer<TTestProof>
        where TTestProof : ITestProof
    {
        public TheoryTestCaseDiscoverer(IMessageSink diagnosticMessageSink) : base(TestCasePropositionType.Theory, diagnosticMessageSink)
        {
            // No additional initialization is needed for FactTestCaseDiscoverer
        }
    }
}
