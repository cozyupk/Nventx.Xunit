using Xunit.Abstractions;

namespace NventX.Xunit.Generic
{
    public class FactTestCaseDiscoverer<TTestProof> : TestCaseForProofDiscoverer<TTestProof>
        where TTestProof : ITestProof
    {
        public FactTestCaseDiscoverer(IMessageSink diagnosticMessageSink) : base(TestCasePropositionType.Fact, diagnosticMessageSink)
        {
            // No additional initialization is needed for FactTestCaseDiscoverer
        }
    }
}
