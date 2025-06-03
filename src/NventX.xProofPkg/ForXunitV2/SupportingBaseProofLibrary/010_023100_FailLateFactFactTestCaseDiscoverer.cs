using NventX.xProof.Abstractions.TestProofForTestRunner;
using Xunit.Abstractions;

namespace NventX.xProof.Xunit.SupportingBaseProofLibrary
{
    internal class FailLateFactFactTestCaseDiscoverer : FactTestCaseDiscoverer<IInvokableProof>
    {
        public FailLateFactFactTestCaseDiscoverer(IMessageSink diagnosticMessageSink)
            : base(diagnosticMessageSink)
        {
            // This discoverer is used to find test cases that use the FailLateFact attribute.
            // It inherits from FactTestCaseDiscoverer with FailLateProof as the proof type.
        }
    }

}